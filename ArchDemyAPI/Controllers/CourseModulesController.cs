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
        [ProducesResponseType(typeof(SuccessResponse<CreateCourseModuleDtos>), 200)]
        public async Task<IActionResult> CreateCourseModule([FromBody] CreateCourseModuleDtos model)
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
        [ProducesResponseType(typeof(SuccessResponse<CreateCourseModuleDtos>), 200)]
        public async Task<IActionResult> AddContentToModule([FromBody] UpdateCourseModuleDto model)
        {
            var response = await _courseModules.AddTopicToCourseModule(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get single course module
        /// </summary>
        /// <param name="courseModuleId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetSingleCourseModule")]
        [ProducesResponseType(typeof(SuccessResponse<GetCourseModuleDto>), 200)]
        public async Task<IActionResult> GetSingleCourseModule([FromQuery] Guid courseModuleId)
        {
            var response = await _courseModules.GetSingleCourseModule(courseModuleId);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to get all course modules
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet()]
        [Route("GetCourseAllModules")]
        [ProducesResponseType(typeof(SuccessResponse<GetCourseModuleDto>), 200)]
        public async Task<IActionResult> GetCourseAllModules([FromQuery] Guid courseId)
        {
            var response = await _courseModules.GetCourseAllModules(courseId);

            return Ok(response);
        }
    }
}
