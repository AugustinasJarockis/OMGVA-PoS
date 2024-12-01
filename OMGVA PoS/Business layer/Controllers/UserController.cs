using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OMGVA_PoS.Business_layer.Services.Security_and_Authorization;
using OMGVA_PoS.Business_layer.Services.UserManagement;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Enums;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Helper_modules.Utilities;
using System.IdentityModel.Tokens.Jwt;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IAuthenticationRepository authenticationRepository, IUserRepository userRepository) : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        private readonly IUserRepository _userRepository = userRepository;

        [HttpPost]
        [ProducesResponseType<User>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // Uncomment this line when all of the admin users have their accounts:
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SignIn([FromBody]SignInRequest signInRequest)
        {
            if (!signInRequest.Email.IsValidEmail())
                return StatusCode(400, "Email is not valid.");

            if (!signInRequest.Username.IsValidName())
                return StatusCode(400, "Name is not valid.");

            if (!signInRequest.Username.IsValidUsername())
                return StatusCode(400, "Username is not valid.");

            if (!signInRequest.Username.IsValidPassword())
                return StatusCode(400, "Password is not valid.");

            if (_authenticationRepository.IsSignedIn(signInRequest.Username, signInRequest.Password))
                return StatusCode(409, "User is already signed in or session exists.");

            if(_authenticationRepository.IsEmailUsed(signInRequest.Email))
                return StatusCode(409, "This email is already in use.");

            if(_authenticationRepository.IsUsernamelUsed(signInRequest.Username))
                return StatusCode(409, "This username is already in use.");

            User user = _authenticationRepository.SignIn(signInRequest);

            if (user == null)
                return StatusCode(500, "Internal server error");

            return Created($"/user/{user.Id}", user);
        }

        [HttpPost("login")]
        [ProducesResponseType<LoginDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginDTO>> Login([FromBody]LoginRequest loginRequest)
        {

            var result = await _authenticationRepository.Login(loginRequest);

            if(!result.IsSuccess)
                return StatusCode(401, result.Message);

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
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Employee) && !token.UserIdEquals(id))
                return Forbid();

            try
            {
                var user = _userRepository.GetUser(id);

                if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals((long)user.BusinessId))
                    return Forbid();

                return Ok(JsonConvert.SerializeObject(user));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                //log the exception with logger
                return StatusCode(500, "Internal server error.");
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
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Employee) && !token.UserIdEquals(id))
                return Forbid();

            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals((long)_userRepository.GetUser(id).BusinessId))
                return Forbid();

            try
            {
                if(user.Email != null && !user.Email.IsValidEmail())
                    return BadRequest();

                _userRepository.UpdateUser(id, user);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception with logger
                return StatusCode(500, "Internal server error.");
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
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);

            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals((long)_userRepository.GetUser(id).BusinessId))
                return Forbid();

            if (token.UserRoleEquals(UserRole.Owner) && token.UserIdEquals(id))
                return Forbid();

            try
            {
                if (_userRepository.DeleteUser(id))
                    return NoContent();
                return NotFound("User not found.");
            }
            catch (Exception ex)
            {
                // Log the exception with logger
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("business/{businessId}")]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<List<User>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBusinessUsers(long businessId)
        {
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(businessId))
                return Forbid();

            var businessUsers = _userRepository.GetBusinessUsers(businessId);

            if(businessUsers == null)
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
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Employee) && !token.UserIdEquals(userId))
                return Forbid();

            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals((long)_userRepository.GetUser(userId).BusinessId))
                return Forbid();

            try
            {
                var schedules = _userRepository.GetUserSchedules(userId);
                return Ok(JsonConvert.SerializeObject(schedules));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception with logger
                return StatusCode(500, "Internal server error.");
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
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Employee) && !token.UserIdEquals(userId))
                return Forbid();

            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals((long)_userRepository.GetUser(userId).BusinessId))
                return Forbid();

            try
            {
                var orders = _userRepository.GetUserOrders(userId);
                return Ok(JsonConvert.SerializeObject(orders));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception with logger
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
