using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Course")]
    [Authorize]

    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Endpoint to create a new course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("New")]
        [ProducesResponseType(typeof(SuccessResponse<CreateCourseDto>), 200)]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseDto model)
        {
            var response = await _courseService.CreateCourse(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get course by course id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetCourseById")]
        [ProducesResponseType(typeof(SuccessResponse<GetCourseDto>), 200)]
        public async Task<IActionResult> GetCourseByCourseId([FromQuery] Guid id)
        {
            var response = await _courseService.GetCourseByCourseId(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get course by course categoryId
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetCourseByCategoryId")]
        [ProducesResponseType(typeof(SuccessResponse<ICollection<CategoryCoursesDto>>), 200)]
        public async Task<IActionResult> GetCoursesByCategoryId([FromQuery] string CategoryId)
        {
            var response = await _courseService.GetCoursesByCategoryId(CategoryId);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get all course
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetAllCourses")]
        //[EnableRateLimiting("getAllCoursePolicy")]
        [ProducesResponseType(typeof(SuccessResponse<ICollection<GetCourseDto>>), 201)]
        //[Authorize]
        public async Task<IActionResult> GetAllCourses()
        {
            var response = await _courseService.GetAllCourses();

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to update course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut()]
        [Route("Update")]
        [ProducesResponseType(typeof(SuccessResponse<UpdateCourseDto>), 200)]
        public async Task<IActionResult> UpdateCourse([FromBody]UpdateCourseDto model)
        {
            var response = await _courseService.UpdateCourse(model);

            return Ok(response);
        }
    }
}
