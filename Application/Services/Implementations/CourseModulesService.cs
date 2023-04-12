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
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class CourseModulesService : ICourseModules
    {
        private readonly IRepository<CourseModule> _courseModuleRepository;
        private readonly IRepository<EachModule> _eachModuleRepository;
        private readonly IRepository<CourseUrl> _courseUrlRepository;
        private readonly IMapper _mapper;
        public CourseModulesService(IRepository<CourseModule> courseModuleRepository, IRepository<EachModule> eachModuleRepository, IRepository<CourseUrl> courseUrlRepository, IMapper mapper)
        {
            _courseModuleRepository = courseModuleRepository;
            _eachModuleRepository = eachModuleRepository;
            _courseUrlRepository = courseUrlRepository;
            _mapper = mapper;
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
