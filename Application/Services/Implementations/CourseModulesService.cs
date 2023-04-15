using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class CourseModulesService : ICourseModules
    {
        private readonly IRepository<CourseModule> _courseModuleRepository;
        private readonly IMapper _mapper;
        public CourseModulesService(IRepository<CourseModule> courseModuleRepository, IMapper mapper)
        {
            _courseModuleRepository = courseModuleRepository;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<CourseModuleDtos>> UpdateCourseModule(CourseModuleDtos model)
        {
            var findCourseModule = await _courseModuleRepository.QueryableEntity(x => x.Id == model.Id)
                .Include(x => x.Topics)
                .SingleOrDefaultAsync();
            
            if (findCourseModule == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.CourseModuleNotFound);
            }

            foreach (var item in model.Topics)
            {
                var extractTopicObject = _mapper.Map<Topic>(item);
                findCourseModule.Topics.Add(extractTopicObject);
            }
            
            var newCourse = _mapper.Map<CourseModule>(findCourseModule);

            _courseModuleRepository.Update(newCourse);
            await _courseModuleRepository.SaveChangesAsync();

            var newCourseResponse = _mapper.Map<CourseModuleDtos>(newCourse);

            return new SuccessResponse<CourseModuleDtos>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<CourseModuleDtos>> CreateCourseModule(CourseModuleDtos model)
        {
            var newCourse = _mapper.Map<CourseModule>(model);
            
            await _courseModuleRepository.AddAsync(newCourse);
            await _courseModuleRepository.SaveChangesAsync();

            var newCourseResponse = _mapper.Map<CourseModuleDtos>(newCourse);

            return new SuccessResponse<CourseModuleDtos>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }
    }
}
