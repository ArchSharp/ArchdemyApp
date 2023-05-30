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
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Stripe;

namespace Application.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<CourseCategory> _categoryRepository;
        private readonly IMapper _mapper;

        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true // Optional: for pretty-printing the JSON output
        };
        public CourseService(
            IRepository<Course> courseRepository,
            IMapper mapper,
            IRepository<User> userRepository,
            IRepository<CourseCategory> categoryRepository
        )
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<SuccessResponse<GetCourseDto>> CreateCourse(CreateCourseDto model)
        {
            var getUser = await _userRepository.FirstOrDefault(x => x.Id == model.AuthorId);
            var newCourse = _mapper.Map<Course>(model);

            if (getUser == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.UserNotFound);

            await _courseRepository.AddAsync(newCourse);
            await _courseRepository.SaveChangesAsync();

            if (!getUser.IsInstructor)
            {
                getUser.IsInstructor = true;
                await _userRepository.SaveChangesAsync();
            }

            var newCourseResponse = _mapper.Map<GetCourseDto>(newCourse);

            return new SuccessResponse<GetCourseDto>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<GetCategoryDTO>> CreateCourseCategory(CreateCategoryDTO model)
        {
            var findCategory = await _categoryRepository.FirstOrDefault(x => x.Name == model.Name);
            var newCategory = _mapper.Map<CourseCategory>(model);

            if (findCategory != null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.CategoryAlreadyExist);

            await _categoryRepository.AddAsync(newCategory);
            await _courseRepository.SaveChangesAsync();

            var newCourseResponse = _mapper.Map<GetCategoryDTO>(newCategory);

            return new SuccessResponse<GetCategoryDTO>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<ICollection<GetAllCategoryDTO>>> GetAllCategories()
        {
            var allCourse = await _categoryRepository.GetAllAsync();

            var courseResponse = _mapper.Map<ICollection<GetAllCategoryDTO>>(allCourse);

            return new SuccessResponse<ICollection<GetAllCategoryDTO>>
            {
                Data = courseResponse,
                code = 201,
                Message = ResponseMessages.CategoriesFetched,
                ExtraInfo = courseResponse.Count() + " records fetched",
            };
        }

        public async Task<SuccessResponse<ICollection<GetCourseDto>>> GetAllCourses()
        {
            var allCourse = await _courseRepository.GetAllAsync();

            var courseResponse = _mapper.Map<ICollection<GetCourseDto>>(allCourse);

            return new SuccessResponse<ICollection<GetCourseDto>>
            {
                Data = courseResponse,
                code = 201,
                Message = ResponseMessages.CourseFetchedSuccesss,
                ExtraInfo = courseResponse.Count()+" records fetched",
            };
        }

        public async Task<SuccessResponse<GetCourseDto>> GetCourseByCourseId(Guid id)
        {
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

        public async Task<SuccessResponse<GetCategoryDTO>> GetCategory(Guid categoryId)
        {
            var findCategory = await _categoryRepository.FirstOrDefault(x => x.CategoryId == categoryId);
            
            if (findCategory == null)
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.CategoryNotFound);            

            var newCourseResponse = _mapper.Map<GetCategoryDTO>(findCategory);

            var getCoursesInCategory = await _courseRepository.FindAsync(x => x.CategoryId == categoryId);
                        
            foreach (var item in getCoursesInCategory)
            {
                //var json = JsonSerializer.Serialize(item, options);
                var courseDto = _mapper.Map<GetCourseDto>(item);
                newCourseResponse.Courses.Add(courseDto);
            }

            return new SuccessResponse<GetCategoryDTO>
            {
                Data = newCourseResponse,
                code = 201,
                Message = ResponseMessages.NewCourseCreated,
                ExtraInfo = "",
            };
        }

        public async Task<SuccessResponse<ICollection<CategoryCoursesDto>>> GetCoursesByCategoryId(Guid CategoryId)
        {
            var findCourse = await _courseRepository.FindAsync(x => x.CategoryId == CategoryId);

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

        public async Task<SuccessResponse<UpdateCourseDto>> UpdateCourse(UpdateCourseDto model)
        {
            var findCourse = await _courseRepository.FirstOrDefault(x => x.CourseId == model.CourseId);

            if (findCourse == null)
            {
                throw new RestException(HttpStatusCode.NotFound, ResponseMessages.CourseNotFound);
            }

            Type type = model.GetType();
            PropertyInfo[] properties = type.GetProperties();

            Type typeDB = findCourse.GetType();
            PropertyInfo[] propertiesDB = typeDB.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string modelName = property.Name;
                object? value = property.GetValue(model);
                foreach (PropertyInfo propertyDB in propertiesDB)
                {
                    string dbPropName = propertyDB.Name;
                    if (dbPropName == "Id") continue;
                    object value2 = propertyDB.GetValue(findCourse)!;
                    if (value != null && modelName == dbPropName && !value.Equals(value2))
                    {
                        PropertyInfo propertyChange = findCourse.GetType().GetProperty(dbPropName)!;
                        if (propertyChange != null && propertyChange.CanWrite)
                        {                           
                                propertyChange.SetValue(findCourse, value);
                        }
                        break;
                    }
                    else if (value != null && modelName == dbPropName && value.Equals(value2))
                    {
                        break;
                    }
                }
            }
            var updatedResponse = _mapper.Map<UpdateCourseDto>(findCourse);
            await _courseRepository.SaveChangesAsync();


            return new SuccessResponse<UpdateCourseDto>
            {
                Data = updatedResponse,
                code = 201,
                Message = ResponseMessages.CourseUpdated,
                ExtraInfo = "",

            };
        }
    }
}
