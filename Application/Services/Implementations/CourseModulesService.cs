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

        public async Task<SuccessResponse<UpdateCourseModuleDto>> AddTopicToCourseModule(UpdateCourseModuleDto model)
        {
            var findCourseModule = await _courseModuleRepository.QueryableEntity(x => x.CourseModuleId == model.CourseModuleId)
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

            var newCourseResponse = _mapper.Map<UpdateCourseModuleDto>(newCourse);

            return new SuccessResponse<UpdateCourseModuleDto>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<CreateCourseModuleDtos>> CreateCourseModule(CreateCourseModuleDtos model)
        {
            var newCourse = _mapper.Map<CourseModule>(model);
            
            await _courseModuleRepository.AddAsync(newCourse);
            await _courseModuleRepository.SaveChangesAsync();

            var newCourseResponse = _mapper.Map<CreateCourseModuleDtos>(newCourse);

            return new SuccessResponse<CreateCourseModuleDtos>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<ICollection<GetCourseModuleDto>>> GetCourseAllModules(Guid courseId)
        {
            List<object> results = new();
            var getThisCourseAllModules = await _courseModuleRepository.FindAsync(x => x.CourseId == courseId);
            foreach (var module in getThisCourseAllModules)
            {
                var findCourseModule = await _courseModuleRepository.QueryableEntity(x => x.CourseModuleId == module.CourseModuleId)
                    .Include(x => x.Topics)
                    .SingleOrDefaultAsync();
                var mapResponse = _mapper.Map<GetCourseModuleDto>(findCourseModule);
                results.Add(mapResponse);
            }
            var response = _mapper.Map<ICollection<GetCourseModuleDto>>(results);

            return new SuccessResponse<ICollection<GetCourseModuleDto>>
            {
                Data = response,
                code = 201,
                Message = "Course Modules" + ResponseMessages.FetchedSuccesss,
                ExtraInfo = results.Count() + " records",
            };
        }

        public async Task<SuccessResponse<GetCourseModuleDto>> GetSingleCourseModule(Guid courseModuleId)
        {
            var findCourseModule = await _courseModuleRepository.QueryableEntity(x => x.CourseModuleId == courseModuleId)
                .Include(x => x.Topics)
                .SingleOrDefaultAsync();

            if (findCourseModule == null)
            {
                throw new RestException(HttpStatusCode.BadRequest, message: ResponseMessages.CourseModuleNotFound);
            }

            var newCourseResponse = _mapper.Map<GetCourseModuleDto>(findCourseModule);

            return new SuccessResponse<GetCourseModuleDto>
            {
                Data = newCourseResponse,
                code = 201,
                Message = "Course Module"+ResponseMessages.FetchedSuccesss,
                ExtraInfo = "",
            };
        }
    }
}
