﻿using Application.DTOs;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICourseModules : IAutoDependencyService
    {
        Task<SuccessResponse<CreateCourseModuleDtos>> CreateCourseModule(CreateCourseModuleDtos model);
        Task<SuccessResponse<UpdateCourseModuleDto>> AddTopicToCourseModule(UpdateCourseModuleDto model);
        Task<SuccessResponse<GetCourseModuleDto>> GetSingleCourseModule(Guid courseModuleId);
        Task<SuccessResponse<ICollection<GetCourseModuleDto>>> GetCourseAllModules(Guid courseId);
    }
}
