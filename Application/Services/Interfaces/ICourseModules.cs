using Application.DTOs;
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
        Task<SuccessResponse<CourseModuleDtos>> CreateCourseModule(CourseModuleDtos model);
        Task<SuccessResponse<CourseModuleDtos>> UpdateCourseModule(CourseModuleDtos model);
    }
}
