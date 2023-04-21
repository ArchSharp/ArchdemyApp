using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
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
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;

        public UserService(IRepository<User> userRepository, IMapper mapper, IJwtService jwtService,IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailService = emailService;
        }
        public async Task<SuccessResponse<CreateUserDto>> Register(CreateUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser != null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserAlreadyExist);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;
            var emailVerifyToken = CreateRandomToken();

            var newUser = _mapper.Map<User>(model);

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();
            var verifyEmailMessage = "Please click the following link " +
                                                       "to verify your email https://localhost:7219/api/v1/Auth/VerifyEmail?email=" 
                                                       + model.Email + "&token=" + emailVerifyToken;
            SendEmailVerificationToken(model.Email, "Email verification", verifyEmailMessage);

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
            string token = _jwtService.CreateJwtToken(findUser);

            if (!findUser.EmailConfirmed)
            {
                TimeSpan timeSinceStartDate = DateTime.Now - findUser.CreatedAt;
                int daysSinceStartDate = timeSinceStartDate.Days;
                if (daysSinceStartDate >= 3)
                {
                    var newVerifyToken = CreateRandomToken();
                    findUser.VerificationToken = newVerifyToken;
                    findUser.VerifiedAt = DateTime.Now.ToUniversalTime();
                    await _userRepository.SaveChangesAsync();

                    var verifyEmailMessage = "Please click the following link " +
                                                       "to verify your email https://localhost:7219/api/v1/Auth/VerifyEmail?email="
                                                       + model.Email + "&token=" + newVerifyToken;
                    SendEmailVerificationToken(model.Email, "Email verification", verifyEmailMessage);
                    responseMessage = "Please check your mail for email verification link";
                    token = "";
                }
                else
                {
                    responseMessage = "Please click on the email verification link in your mail";
                    token = "";
                }
                //throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.VerifyEmail);
            }
                        
            var userResponse = _mapper.Map<CreateUserDto>(findUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = responseMessage,
                ExtraInfo = token,
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

    }
}
