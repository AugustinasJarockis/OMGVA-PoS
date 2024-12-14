using System.Net;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.AuthManagement.Service;
using OmgvaPOS.HelperUtils;

namespace OmgvaPOS.AuthManagement.Controller
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("login")]
        [ProducesResponseType<LoginResponseDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var login = _authService.Login(loginRequest);

            if (!login.IsSuccess)
                return Unauthorized(login);

            Response.Headers.Append("Authorization", "Bearer " + login.Token);
            return Ok(login);
        }

        [HttpGet("login/{businessId}")]
        [ProducesResponseType<LoginResponseDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Login(long businessId)
        {
            var currentToken = JwtTokenHandler.GetTokenDetails(HttpContext.Request.Headers.Authorization!);
            var login = _authService.LoginAdminWithDifferentBusiness(businessId, currentToken);
            
            Response.Headers.Append("Authorization", "Bearer " + login.Token);
            return Ok(login);
        }
    }
}
