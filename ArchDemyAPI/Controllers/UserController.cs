using Application.DTOs;
using Application.Helpers;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/Auth")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly ITwilioService _twilioService;

        public UserController(
            IUserService userService,
            IJwtService jwtService,
            ITwoFactorAuthService twoFactorAuthService,
            ITwilioService twilioService
        )
        {
            _userService = userService;
            _jwtService = jwtService;
            _twoFactorAuthService = twoFactorAuthService;
            _twilioService = twilioService;
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
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("VerifyEmail")]
        [ProducesResponseType(typeof(SuccessResponse<CreateUserDto>), 200)]
        public async Task<IActionResult> VerifyEmail([FromQuery]string email,[FromQuery] string token)
        {
            var response = await _userService.VerifyEmail(email,token);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to renew token
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

        /// <summary>
        /// Endpoint to Google two factor authentication
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("Google2FA")]
        [ProducesResponseType(typeof(SuccessResponse<GoogleTwoFactorAuthResponse>), 200)]
        public async Task<IActionResult> GoogleTwoFactorAuth([FromQuery] string email)
        {
            var response = await _twoFactorAuthService.GenerateNewSecretKey(email);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to verify  Google2FA pin
        /// </summary>
        /// <param name="email"></param>
        /// /// <param name="twoFACode"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("VerifyGoogle2FA")]
        [ProducesResponseType(typeof(SuccessResponse<string>), 200)]
        public async Task<IActionResult> VerifyGoogleTwoFactorAuth([FromQuery] string email, [FromQuery] string twoFACode)
        {
            var response = await _twoFactorAuthService.ValidateTwoFactorPin(email, twoFACode);
            return Ok(response);
        }

        /// <summary>
        /// Endpoint to send OTP using Twilio
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        [Route("TwilioSendOTP")]
        public async Task<IActionResult> TwilioSendMessageAync([FromBody] TwilioRequestDto model)
        {

            var response = await _twilioService.TwilioSendAsync(model.Message, model.To);

            return Ok(response);
        }
    }
}
