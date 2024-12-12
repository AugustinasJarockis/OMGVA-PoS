using Microsoft.AspNetCore.Mvc;
using System.Net;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.Validators;
using OmgvaPOS.AuthManagement.Service;
using OmgvaPOS.HelperUtils;
using System.IdentityModel.Tokens.Jwt;
using OmgvaPOS.UserManagement.Enums;

namespace src.AuthManagement.Controller
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("signup")]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Uncomment this line when all of the admin users have their accounts:
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignUp([FromBody] SignUpRequest signUpRequest)
        {
            if (!signUpRequest.Email.IsValidEmail())
                return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid.");

            if (!signUpRequest.Name.IsValidName())
                return StatusCode((int)HttpStatusCode.BadRequest, "Name is not valid.");

            if (!signUpRequest.Username.IsValidUsername())
                return StatusCode((int)HttpStatusCode.BadRequest, "Username is not valid.");

            if (!signUpRequest.Password.IsValidPassword())
                return StatusCode((int)HttpStatusCode.BadRequest, "Password is not valid.");

            if (_authService.IsSignedUp(signUpRequest.Username, signUpRequest.Password))
                return StatusCode((int)HttpStatusCode.Conflict, "User is already signed up or session exists.");

            if (_authService.IsEmailUsed(signUpRequest.Email))
                return StatusCode((int)HttpStatusCode.Conflict, "This email is already in use.");

            if (_authService.IsUsernameUsed(signUpRequest.Username))
                return StatusCode((int)HttpStatusCode.Conflict, "This username is already in use.");

            User user = _authService.SignUp(signUpRequest);

            if (user == null)
            {
                _logger.LogError("An unexpected internal server error occured while creating user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }

            return Created($"/user/{user.Id}", user);
        }

        [HttpPost("login")]
        [ProducesResponseType<LoginDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {

            var result = _authService.Login(loginRequest);

            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.Unauthorized, result.Message);

            Response.Headers.Add("Authorization", "Bearer " + result.Token);

            return Ok(result);
        }

        [HttpGet("login/{businessId}")]
        [ProducesResponseType<LoginDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Login(long businessId)
        {

            var result = _authService.GenerateAdminJwtToken(businessId, JwtTokenHandler.GetTokenDetails(HttpContext.Request.Headers.Authorization));

            if (!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.Unauthorized, result.Message);

            Response.Headers.Add("Authorization", "Bearer " + result.Token);

            return Ok(result);
        }
    }
}
