using Application.DTOs;
using Application.Helpers;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[ApiController]
    //[ApiVersion("1.0")]
    [Route("api/CourseModule")]
    public class CourseModulesController : Controller
    {
        private readonly ICourseModules _courseModules;
        public CourseModulesController(ICourseModules courseModules)
        {
             _courseModules = courseModules;
        }

        /// <summary>
        /// Endpoint to create a new course module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("NewCourse")]
        [ProducesResponseType(typeof(SuccessResponse<CourseModuleDtos>), 200)]
        public async Task<IActionResult> CreateCourseModule([FromBody] CourseModuleDtos model)
        {
            var response = await _courseModules.CreateCourseModule(model);

            return Ok(response);
        }
    }
}
