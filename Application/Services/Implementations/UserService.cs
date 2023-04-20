﻿using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;

        public UserService(IRepository<User> userRepository, IMapper mapper, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<SuccessResponse<ChangePasswordDto>> ChangePassword(ChangePasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;

            var newUser = _mapper.Map<User>(model);
            _mapper.Map(model, findUser);
            await _userRepository.SaveChangesAsync();

            var userResponse = _mapper.Map<ChangePasswordDto>(newUser);            

            return new SuccessResponse<ChangePasswordDto>
            {
                Data = userResponse,
                code = 200,
                Message = ResponseMessages.LoginSuccessful,
                ExtraInfo = ""
            };
        }

        public async Task<SuccessResponse<CreateUserDto>> CreateUser(CreateUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser != null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserAlreadyExist);

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashPassword;

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

        public async Task<SuccessResponse<ForgotPasswordDto>> ForgotPassword(ForgotPasswordDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x => x.Email == model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            return new SuccessResponse<ForgotPasswordDto>
            {
                code = 201,
                Message = ResponseMessages.ForgotPasswordLinkSent,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<CreateUserDto>> LoginUser(LoginUserDto model)
        {
            var findUser = await _userRepository.FirstOrDefault(x=>x.Email== model.Email);

            if (findUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);
            if (!BCrypt.Net.BCrypt.Verify(model.Password, findUser.Password))
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.InCorrectPassword);
            string token = _jwtService.CreateJwtToken(findUser);            

            var userResponse = _mapper.Map<CreateUserDto>(findUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data = userResponse,
                code = 200,
                Message = ResponseMessages.LoginSuccessful,
                ExtraInfo = token,
            };
        }
    }
}
