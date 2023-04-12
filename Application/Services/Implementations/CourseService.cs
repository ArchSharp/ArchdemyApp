using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IMapper _mapper;
        public CourseService(IRepository<Course> courseRepository, IMapper mapper) { 
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<CreateCourseDto>> CreateCourse(CreateCourseDto model)
        {
            var newCourse = _mapper.Map<Course>(model);

            await _courseRepository.AddAsync(newCourse);
            await _courseRepository.SaveChangesAsync();

            var newCourseResponse = _mapper.Map<CreateCourseDto>(newCourse);

            return new SuccessResponse<CreateCourseDto>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<GetCourseDto>> GetCourseByCourseId(Guid id)
        {
            //var findCourse = await _courseRepository.FirstOrDefault(x => x.courseId == id);
            var findCourse = await _courseRepository.GetByIdAsync(id);

            if (findCourse == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.CourseNotFound);

            var courseResponse = _mapper.Map<GetCourseDto>(findCourse);

            return new SuccessResponse<GetCourseDto>
            {
                Data = courseResponse,
                code = 200,
                Message = ResponseMessages.CourseFetchedSuccesss,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<ICollection<CategoryCoursesDto>>> GetCoursesByCategoryId(string catId)
        {
            var findCourse = await _courseRepository.FindAsync(x => x.categoryId == catId);

            if (findCourse == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.CourseNotFound);

            var courseResponse = _mapper.Map<ICollection<CategoryCoursesDto>>(findCourse);

            return new SuccessResponse<ICollection<CategoryCoursesDto>>
            {
                Data = courseResponse,
                code = 200,
                Message = ResponseMessages.CourseFetchedSuccesss,
                ExtraInfo = courseResponse.Count()+" records",
            };
        }
    }
}
