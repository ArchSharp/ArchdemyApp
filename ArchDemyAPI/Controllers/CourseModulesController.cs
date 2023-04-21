using Application.DTOs;
using Application.Helpers;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/CourseModule")]
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
        [Route("New")]
        [ProducesResponseType(typeof(SuccessResponse<CourseModuleDtos>), 200)]
        public async Task<IActionResult> CreateCourseModule([FromBody] CourseModuleDtos model)
        {
            var response = await _courseModules.CreateCourseModule(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to add course to module
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut()]
        [Route("AddTopic")]
        [ProducesResponseType(typeof(SuccessResponse<CourseModuleDtos>), 200)]
        public async Task<IActionResult> AddContentToModule([FromBody] CourseModuleDtos model)
        {
            var response = await _courseModules.UpdateCourseModule(model);

            return Ok(response);
        }
    }
}
