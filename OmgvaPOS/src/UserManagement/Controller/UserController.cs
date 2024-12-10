using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmgvaPOS.AuthManagement.Repository;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Enums;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.UserManagement.Repository;
using OmgvaPOS.Validators;

namespace OmgvaPOS.UserManagement.Controller
{
    [ApiController]
    [Route("user")]
    public class UserController(IAuthenticationRepository authenticationRepository, IUserRepository userRepository, ILogger<UserController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Uncomment this line when all of the admin users have their accounts:
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignIn([FromBody]SignInRequest signInRequest) //sita reiks padaryti, kad tik admin ar owneriui butu butu
        {
            if (!signInRequest.Email.IsValidEmail())
                return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid.");

            if (!signInRequest.Username.IsValidName())
                return StatusCode((int)HttpStatusCode.BadRequest, "Name is not valid.");

            if (!signInRequest.Username.IsValidUsername())
                return StatusCode((int)HttpStatusCode.BadRequest, "Username is not valid.");

            if (!signInRequest.Username.IsValidPassword())
                return StatusCode((int)HttpStatusCode.BadRequest, "Password is not valid.");

            if (_authenticationRepository.IsSignedIn(signInRequest.Username, signInRequest.Password))
                return StatusCode((int)HttpStatusCode.Conflict, "User is already signed in or session exists.");

            if(_authenticationRepository.IsEmailUsed(signInRequest.Email))
                return StatusCode((int)HttpStatusCode.Conflict, "This email is already in use.");

            if(_authenticationRepository.IsUsernamelUsed(signInRequest.Username))
                return StatusCode((int)HttpStatusCode.Conflict, "This username is already in use.");

            User user = _authenticationRepository.SignIn(signInRequest);

            if (user == null) {
                _logger.LogError("An unexpected internal server error occured while creating user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }

            return Created($"/user/{user.Id}", user);
        }

        [HttpPost("login")]
        [ProducesResponseType<LoginDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginDTO>> Login([FromBody]LoginRequest loginRequest)
        {

            var result = await _authenticationRepository.Login(loginRequest);

            if(!result.IsSuccess)
                return StatusCode((int)HttpStatusCode.Unauthorized, result.Message);

            Response.Headers.Add("Authorization", "Bearer " + result.Token);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            return Ok(JsonConvert.SerializeObject(_userRepository.GetUsers()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<User>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUser(long id)
        {
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoExcept(id)?.BusinessId, id))
                return Forbid();

            try
            {
                var user = _userRepository.GetUser(id);
                return Ok(JsonConvert.SerializeObject(user));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected internal server error occured while getting user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Update([FromBody] UpdateUserRequest user, long id)
        {
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoExcept(id)?.BusinessId, id))
                return Forbid();

            try
            {
                if(!user.Email.IsValidEmail())
                    return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid.");

                _userRepository.UpdateUser(id, user);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected internal server error occured while updating user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(long id)
        {
            if (!JwtTokenHandler.CanDeleteUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoExcept(id)?.BusinessId, id))
                return Forbid();

            try
            {
                if (_userRepository.DeleteUser(id))
                    return NoContent();
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected internal server error occured while deleting user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("business/{businessId}")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBusinessUsers(long businessId)
        {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, businessId))
                return Forbid();

            var businessUsers = _userRepository.GetBusinessUsers(businessId);

            if (businessUsers == null)
                return NotFound();

            return Ok(JsonConvert.SerializeObject(businessUsers));
        }

        [HttpGet("{userId}/schedules")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<EmployeeSchedule>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserSchedules(long userId)
        {
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoExcept(userId)?.BusinessId, userId))
                return Forbid();

            try
            {
                var schedules = _userRepository.GetUserSchedules(userId);
                return Ok(JsonConvert.SerializeObject(schedules));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected internal server error occured while getting user schedules.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("{userId}/order")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<Order>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserOrders(long userId)
        {
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoExcept(userId)?.BusinessId, userId))
                return Forbid();

            try
            {
                var orders = _userRepository.GetUserOrders(userId);
                return Ok(JsonConvert.SerializeObject(orders));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected internal server error occured while getting user orders.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }
    }
}
