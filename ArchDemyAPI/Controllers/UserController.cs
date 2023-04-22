using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Auth")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Endpoint to create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("Register")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 201)]
        public async Task<IActionResult> Register(CreateUserDto model)
        {
            var response = await _userService.Register(model);

            return Ok(response);
        }

        /// <summary>
        /// Endpoint to user login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("Login")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> Login([FromBody]LoginUserDto model)
        {
            var response = await _userService.Login(model);
            return Ok(response);
        }

        /// <summary>
        /// Forgot password endpoint
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("ForgotPassword")]
        [ProducesResponseType(typeof(SuccessResponse<ForgotPasswordDto>), 204)]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDto model)
        {
            var response = await _userService.ForgotPassword(model);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("ResetPassword")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var response = await _userService.ResetPassword(model);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to change password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("ChangePassword")]
        [ProducesResponseType(typeof(SuccessResponse<ChangePasswordDto>), 200)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var response = await _userService.ChangePassword(model);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to verify email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("VerifyEmail")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> VerifyEmail(string email, string token)
        {
            var response = await _userService.VerifyEmail(email,token);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to verify email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("RenewToken")]
        [ProducesResponseType(typeof(SuccessResponse<TokenDto>), 200)]
        public async Task<IActionResult> RenewToken(RefreshTokenDto model)
        {
            var response = await _jwtService.RenewTokens(model);
            return Ok(response);
        }
    }
}
