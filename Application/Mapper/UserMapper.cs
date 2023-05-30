using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public class UserMapper :Profile
    {
        public UserMapper()
        {
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
            CreateMap<User, ChangePasswordDto>().ReverseMap();
            CreateMap<User, ResetPasswordDto>().ReverseMap();
        }
    }
}
