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
    internal class CourseMapper : Profile
    {
        public CourseMapper()
        {
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<CourseCategory, CreateCategoryDTO>().ReverseMap();
            CreateMap<CourseCategory, GetCategoryDTO>().ReverseMap();
            CreateMap<CourseCategory, GetAllCategoryDTO>().ReverseMap();
            CreateMap<Course, GetCourseDto>();
            CreateMap<Course, CategoryCoursesDto>();
            CreateMap<Course, UpdateCourseDto>().ReverseMap();
        }
    }
}
