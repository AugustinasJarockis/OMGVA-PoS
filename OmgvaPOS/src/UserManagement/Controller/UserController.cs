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
using OmgvaPOS.UserManagement.Repository;

namespace OmgvaPOS.UserManagement.Controller
{
    [ApiController]
    [Route("user")]
    public class UserController(IUserService userService, IAuthService authService, IUserRepository userRepository, ILogger<UserController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<UserController> _logger = logger;

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            
            UserResponse user = _userService.CreateUser(createUserRequest);
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
            return Ok(_userService.GetAllUsers());
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
            if (!AuthorizationHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            var user = _userService.GetUser(id);
            return Ok(user);
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
            if (!AuthorizationHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            if (!user.Email.IsValidEmail())
                return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid.");

            _userService.UpdateUser(id, user);
            return Ok();
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
            if (!AuthorizationHandler.CanDeleteUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(id)?.BusinessId, id))
                return Forbid();

            if (_userService.DeleteUser(id))
                return NoContent();
            
            return NotFound("User not found.");
        }

        [HttpGet("business/{businessId}")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBusinessUsers(long businessId)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, businessId))
                return Forbid();

            var businessUsers = _userService.GetBusinessUsers(businessId);

            if (businessUsers == null)
                return NotFound();

            return Ok(businessUsers);
        }

        [HttpGet("{userId}/schedules")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<EmployeeSchedule>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserSchedules(long userId)
        {
            if (!AuthorizationHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(userId)?.BusinessId, userId))
                return Forbid();

            var schedules = _userService.GetUserSchedules(userId);
            return Ok(schedules);
        }

        [HttpGet("{userId}/order")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<Order>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserOrders(long userId)
        {
            if (!AuthorizationHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(userId)?.BusinessId, userId))
                return Forbid();

            var orders = _userService.GetUserOrders(userId);
            return Ok(orders);
        }
    }
}
