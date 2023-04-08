using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/auth")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Endpoint to create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("NewUser")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> CreateUser(CreateUserDto model)
        {
            var response = await _userService.CreateUser(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to user login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("LoginUser")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> LoginUser([FromBody]LoginUserDto model)
        {
            var response = await _userService.LoginUser(model);
            return Ok(response);
        }

        [HttpPost()]
        [Route("ForgotPassword")]
        [ProducesResponseType(typeof(SuccessResponse<ForgotPasswordDto>), 200)]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDto model)
        {
            var response = await _userService.ForgotPassword(model);
            return Ok(response);
        }
    }
}
