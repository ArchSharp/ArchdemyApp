﻿using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Data.Migrations;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserService(IRepository<User> userRepository, IMapper mapper, IJwtService jwtService, IEmailService emailService, IRepository<RefreshToken> refreshTokenRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailService = emailService;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<SuccessResponse<CreateUserDto>> Register(CreateUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser != null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserAlreadyExist);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;

            var emailVerifyToken = CreateRandomToken();
            var verifyEmailMessage = "Please click the following link " +
                                                       "to verify your email https://localhost:7219/api/v1/Auth/VerifyEmail?email=" 
                                                       + model.Email + "&token=" + emailVerifyToken;
            SendEmailVerificationToken(model.Email, "Email verification", verifyEmailMessage);

            var newUser = _mapper.Map<User>(model);

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var newUserResponse = _mapper.Map<CreateUserDto>(newUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data = newUserResponse,
                code = 201,
                Message = ResponseMessages.NewUserCreated,
                ExtraInfo = "",
            };
        }
        public async Task<SuccessResponse<CreateUserDto>> Login(LoginUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x=>x.Email== model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(model.Password, findUser.Password))
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.InCorrectPassword);

            var responseMessage = ResponseMessages.LoginSuccessful;
            
            // Create user Token
            string accessToken = _jwtService.CreateJwtToken(findUser);
            var isUserRefreshTokenInDb = await _refreshTokenRepository.FirstOrDefault(x => x.UserId == findUser.Id && x.ExpirationDate >= DateTime.UtcNow);
            var refreshToken = isUserRefreshTokenInDb == null ? await _jwtService.CreateRefreshToken() : isUserRefreshTokenInDb.Token;
            if(isUserRefreshTokenInDb == null) 
                await InsertRefreshToken(findUser.Id, refreshToken);
            
            var extraIfo = new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            //----------------------

            if (!findUser.EmailConfirmed)
            {
                TimeSpan timeSinceStartDate = DateTime.UtcNow - findUser.CreatedAt;
                int daysSinceStartDate = timeSinceStartDate.Days;
                if (daysSinceStartDate >= 3)
                {
                    var newVerifyToken = CreateRandomToken();
                    findUser.VerificationToken = newVerifyToken;
                    findUser.VerifiedAt = DateTime.UtcNow;
                    await _userRepository.SaveChangesAsync();

                    var verifyEmailMessage = "Please click the following link " +
                                                       "to verify your email https://localhost:7219/api/v1/Auth/VerifyEmail?email="
                                                       + model.Email + "&token=" + newVerifyToken;
                    SendEmailVerificationToken(model.Email, "Email verification", verifyEmailMessage);

                    throw new RestException(HttpStatusCode.Forbidden, ResponseMessages.UserEmailNotVerified);
                }
                else
                {
                    throw new RestException(HttpStatusCode.Forbidden, ResponseMessages.UserEmailNotVerified);
                }                
            }

            var userResponse = _mapper.Map<CreateUserDto>(findUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = responseMessage,
                ExtraInfo = extraIfo,
            };
        }
        public async Task<SuccessResponse<ForgotPasswordDto>> ForgotPassword(ForgotPasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);
            
            var resetToken = CreateRandomToken();
            var verifyEmailMessage = "Please click the following link " +
                                                       "to reset your password https://localhost:7219/api/v1/Auth/ResetPassword?email="
                                                       + model.Email + "&token=" + resetToken;
            SendEmailVerificationToken(model.Email, "Password reset", verifyEmailMessage);
            
            findUser.PasswordResetToken = resetToken;
            findUser.ResetTokenExpires= DateTime.UtcNow.ToUniversalTime().AddDays(1);
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<ForgotPasswordDto>
            {
                code = 204,
                Message = ResponseMessages.ForgotPasswordLinkSent,
                ExtraInfo = "",
            };
        }
        public async Task<SuccessResponse<CreateUserDto>> ResetPassword(ResetPasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email && x.PasswordResetToken == model.Token);

            if (findUser == null || findUser.ResetTokenExpires < DateTime.Now.ToUniversalTime())
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);
                                   
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;
            findUser.PasswordResetToken = null;
            findUser.ResetTokenExpires = null;

            var newUser = _mapper.Map<User>(model);
            _mapper.Map(model, findUser);
            await _userRepository.SaveChangesAsync();

            var userResponse = _mapper.Map<CreateUserDto>(findUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = ResponseMessages.ResetSuccessful,
                ExtraInfo = ""
            };
        }
        public async Task<SuccessResponse<CreateUserDto>> ChangePassword(ChangePasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, findUser.Password))
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.InCorrectPassword);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            model.NewPassword = hashPassword;

            //var newUser = _mapper.Map<User>(model);
            //_mapper.Map(model, findUser);
            findUser.Password = model.NewPassword;
            await _userRepository.SaveChangesAsync();

            var userResponse = _mapper.Map<CreateUserDto>(findUser);            

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = ResponseMessages.PasswordChangedSuccessful,
                ExtraInfo = ""
            };
        }
        public async Task<SuccessResponse<CreateUserDto>> VerifyEmail(string email, string token)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.VerificationToken == token && x.Email == email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.InvalidToken);

            var userResponse = _mapper.Map<CreateUserDto>(findUser);
            findUser.UpdatedAt = DateTime.Now.ToUniversalTime();
            findUser.EmailConfirmed = true;
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = ResponseMessages.VerifiedEmail,
                ExtraInfo = "",
            };
        }
        public async Task<SuccessResponse<TokenDto>> RenewTokens(RefreshTokenDto model)
        {
            var tokens = await _jwtService.RenewTokens(model);
            if (tokens == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.InvalidRefreshToken);
            }

            return new SuccessResponse<TokenDto>
            {
                Data = tokens,
                code = 200,
                Message = ResponseMessages.RenewedToken,
                ExtraInfo = "",
            };
        }
        private string CreateRandomToken ()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        private void SendEmailVerificationToken(string receiverEmail, string subject, string body)
        {
            try
            {
                _emailService.SendEmail(receiverEmail, subject, body);
            }
            catch (Exception ex)
            {
                throw new RestException(HttpStatusCode.NotFound, ex.ToString());
            }
        }
        private async Task InsertRefreshToken(Guid userId, string refreshtoken)
        {
            var newRefreshToken = new RefreshToken
            {
                UserId = userId,
                Token = refreshtoken,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };
            await _refreshTokenRepository.AddAsync(newRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();
        }
    }
}
