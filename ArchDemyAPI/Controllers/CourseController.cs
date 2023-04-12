using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[ApiController]
    //[ApiVersion("1.0")]
    [Route("api/Course")]
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
        [Route("NewCourse")]
        [ProducesResponseType(typeof(SuccessResponse<CreateCourseDto>), 200)]
        public async Task<IActionResult> CreateCourse([FromBody]CreateCourseDto model)
        {
            var response = await _courseService.CreateCourse(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get course by course id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetCourseByCourseId")]
        [ProducesResponseType(typeof(SuccessResponse<GetCourseDto>), 200)]
        public async Task<IActionResult> GetCourseByCourseId([FromQuery] Guid id)
        {
            var response = await _courseService.GetCourseByCourseId(id);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get course by course categoryId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetCourseByCategoryId")]
        [ProducesResponseType(typeof(SuccessResponse<ICollection<CategoryCoursesDto>>), 200)]
        public async Task<IActionResult> GetCoursesByCategoryId([FromQuery] string catId)
        {
            var response = await _courseService.GetCoursesByCategoryId(catId);

            return Ok(response);
        }
    }
}
