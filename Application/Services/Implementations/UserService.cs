using Application.DTOs;
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
        public UserService(IRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<CreateUserDto>> CreateUser(CreateUserDto model)
        {
            var newUser = _mapper.Map<User>(model);

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var newUserResponse = _mapper.Map<CreateUserDto>(newUser);

            return new SuccessResponse<CreateUserDto>
            {
                Data= newUserResponse,
                code = 201,
                Message = ResponseMessages.NewUserCreated,
                ExtraInfo = "",                
            };
        }
    }
}
