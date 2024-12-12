using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.OrderManagement.Models;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.UserManagement.DTOs;
using OmgvaPOS.UserManagement.Models;
using OmgvaPOS.Validators;
using OmgvaPOS.UserManagement.Service;
using OmgvaPOS.AuthManagement.DTOs;
using OmgvaPOS.AuthManagement.Service;

namespace OmgvaPOS.UserManagement.Controller
{
    [ApiController]
    [Route("user")]
    public class UserController(IUserService userService, IAuthService authService, ILogger<UserController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // TODO: Uncomment this line when all of the admin users have their accounts:
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] SignUpRequest signUpRequest)
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

            UserResponse user = _userService.CreateUser(signUpRequest);

            if (user == null)
            {
                _logger.LogError("An unexpected internal server error occured while creating user.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
            }

            return Created($"/user/{user.Id}", user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers()
        {
            return Ok(JsonConvert.SerializeObject(_userService.GetAllUsers()));
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
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            try
            {
                var user = _userService.GetUser(id);
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
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            try
            {
                if (!user.Email.IsValidEmail())
                    return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid.");

                _userService.UpdateUser(id, user);
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
            if (!JwtTokenHandler.CanDeleteUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            try
            {
                if (_userService.DeleteUser(id))
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

            var businessUsers = _userService.GetBusinessUsers(businessId);

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
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(userId)?.BusinessId, userId))
                return Forbid();

            try
            {
                var schedules = _userService.GetUserSchedules(userId);
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
            if (!JwtTokenHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(userId)?.BusinessId, userId))
                return Forbid();

            try
            {
                var orders = _userService.GetUserOrders(userId);
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
