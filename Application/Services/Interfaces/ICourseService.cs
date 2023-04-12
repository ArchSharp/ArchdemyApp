using Application.DTOs;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICourseService : IAutoDependencyService
    {
        Task<SuccessResponse<CreateCourseDto>> CreateCourse(CreateCourseDto model);
        Task<SuccessResponse<GetCourseDto>> GetCourseByCourseId(Guid id);
        Task<SuccessResponse<ICollection<CategoryCoursesDto>>> GetCoursesByCategoryId(string catId);
    }
}
