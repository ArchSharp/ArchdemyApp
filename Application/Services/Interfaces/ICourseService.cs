using Application.DTOs;
using Application.Helpers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface ICourseService : IAutoDependencyService
    {
        Task<SuccessResponse<GetCourseDto>> CreateCourse(CreateCourseDto model);
        Task<SuccessResponse<GetCourseDto>> GetCourseByCourseId(Guid id);
        Task<SuccessResponse<ICollection<CategoryCoursesDto>>> GetCoursesByCategoryId(Guid CategoryId);
        Task<SuccessResponse<ICollection<GetCourseDto>>> GetAllCourses();
        Task<SuccessResponse<UpdateCourseDto>> UpdateCourse(UpdateCourseDto model);
        Task<SuccessResponse<GetCategoryDTO>> CreateCourseCategory(CreateCategoryDTO model);
        Task<SuccessResponse<GetCategoryDTO>> GetCategory(Guid categoryId);
        Task<SuccessResponse<ICollection<GetAllCategoryDTO>>> GetAllCategories();
    }
}
