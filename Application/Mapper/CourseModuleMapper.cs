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
    internal class CourseModuleMapper : Profile
    {
        public CourseModuleMapper() { 
            CreateMap<CourseModule, CreateCourseModuleDtos>().ReverseMap();            
            CreateMap<Topic, TopicDto>().ReverseMap();
            CreateMap<CourseModule, UpdateCourseModuleDto>().ReverseMap();
            CreateMap<CourseModule, GetCourseModuleDto>().ReverseMap();
        }
    }
}
