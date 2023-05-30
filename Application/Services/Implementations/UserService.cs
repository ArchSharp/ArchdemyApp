using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Token;
//using Infrastructure.Data.Migrations;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//using System.Web.Mvc;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;

        public UserService(
            IRepository<User> userRepository,
            IMapper mapper, IJwtService jwtService,
            IEmailService emailService,
            IRepository<RefreshToken> refreshTokenRepository,
            ITwoFactorAuthService twoFactorAuthService,
            INotificationService notificationService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
            _emailService = emailService;
            _refreshTokenRepository = refreshTokenRepository;
            _notificationService = notificationService;
            _twoFactorAuthService = twoFactorAuthService;
        }
        public async Task<SuccessResponse<GetUserDto>> Register(CreateUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser != null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserAlreadyExist);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;

            var emailVerifyToken = CreateRandomToken();            

            var newUser = _mapper.Map<User>(model);
            newUser.VerificationToken= emailVerifyToken;

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            _notificationService.SendVerificationEmail(newUser.Email, newUser.LastName, emailVerifyToken);
            var newUserResponse = _mapper.Map<GetUserDto>(newUser);

            return new SuccessResponse<GetUserDto>
            {
                Data = newUserResponse,
                code = 201,
                Message = ResponseMessages.NewUserCreated,
                ExtraInfo = "",
            };
        }
        public async Task<SuccessResponse<GetUserDto>> Login(LoginUserDto model)
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
                    var emailVerifyToken = CreateRandomToken();
                    findUser.VerificationToken = emailVerifyToken;
                    findUser.VerifiedAt = DateTime.UtcNow;
                    await _userRepository.SaveChangesAsync();
                    _notificationService.SendVerificationEmail(findUser.Email, findUser.LastName, emailVerifyToken);

                    throw new RestException(HttpStatusCode.Forbidden, ResponseMessages.UserEmailNotVerified);
                }
                else
                {
                    throw new RestException(HttpStatusCode.Forbidden, ResponseMessages.UserEmailNotVerified);
                }                
            }

            var userResponse = _mapper.Map<GetUserDto>(findUser);

            return new SuccessResponse<GetUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = responseMessage,
                ExtraInfo = extraIfo,
            };            
        }
        public async Task<SuccessResponse<UpdateUserDto>> UpdateUser(string email, UpdateUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Type typeDB = findUser.GetType();
            PropertyInfo[] propertiesDB = typeDB.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string modelName = property.Name;
                object? value = property.GetValue(model);
                foreach (PropertyInfo propertyDB in propertiesDB)
                {
                    string dbPropName = propertyDB.Name;
                    if (dbPropName == "Id") continue;
                    object value2 = propertyDB.GetValue(findUser)!;
                    if (value != null && modelName == dbPropName && !value.Equals(value2))
                    {
                        PropertyInfo propertyChange = findUser.GetType().GetProperty(dbPropName)!;
                        if (propertyChange != null && propertyChange.CanWrite)
                        {
                            if(value.GetType() == typeof(List<string>))
                            {
                                List<string> valueList = new List<string>();
                                var objToList = (List<string>)value;
                                var modelToList = (List<string>)value2;
                                foreach (var item in objToList)
                                {
                                    //valueList.Add(item.ToString());
                                    modelToList.Add(item.ToString());
                                }
                                propertyChange.SetValue(findUser, modelToList);
                                //Console.WriteLine($"{property.Name}: {value}, value2: {findUser} v: {objToList}");
                                break;
                            }
                            else
                            {
                                propertyChange.SetValue(findUser, value);
                                //Console.WriteLine($"{property.Name}: {value}, value1: {findUser} t1: {value.GetType()} t2: {typeof(List<string>)}");
                            }
                        }
                        break;
                    }else if (value != null && modelName == dbPropName && value.Equals(value2))
                    {
                        break;
                    }
                }
            }
                        
            await _userRepository.SaveChangesAsync();

            return new SuccessResponse<UpdateUserDto>
            {
                Data = null,
                code = 201,
                Message = ResponseMessages.CourseUpdated,
                ExtraInfo = "",

            };
        }
        public async Task<SuccessResponse<ForgotPasswordDto>> ForgotPassword(ForgotPasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);
            
            var resetToken = CreateRandomToken();            
            
            findUser.PasswordResetToken = resetToken;
            findUser.ResetTokenExpires= DateTime.UtcNow.ToUniversalTime().AddDays(1);
            await _userRepository.SaveChangesAsync();

            _notificationService.SendPasswordResetEmail(findUser.Email, findUser.LastName, resetToken);

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
            findUser.VerifiedAt = DateTime.UtcNow;
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
