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
        }
    }
}
