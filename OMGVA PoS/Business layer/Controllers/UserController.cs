using Microsoft.AspNetCore.Mvc;
using OMGVA_PoS.Business_layer.Services.Security_and_Authorization;
using OMGVA_PoS.Business_layer.Services.UserManagement;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Helper_modules.Utilities;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController(IUserAuthenticationRepository userAuthenticationRepository, IUserRepository userRepository) : Controller
    {
        private readonly IUserAuthenticationRepository _userAuthenticationRepository = userAuthenticationRepository;
        private readonly IUserRepository _userRepository = userRepository;
        [HttpPost]
        public IActionResult SignIn([FromBody]SignInModel signInModel)
        {
            if(_userAuthenticationRepository.IsSignedIn(signInModel.Username, signInModel.Password))
                return StatusCode(409, "User is already signed in or session exists.");

            if(_userAuthenticationRepository.IsEmailUsed(signInModel.Email))
                return StatusCode(409, "This email is already in use.");

            if(_userAuthenticationRepository.IsUsernamelUsed(signInModel.Username))
                return StatusCode(409, "This username is already in use.");

            User user = _userAuthenticationRepository.SignIn(signInModel);

            if (user == null)
                return StatusCode(500, "Internal server error");

            return Created($"/user/{user.Id}", user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginDTO>> Login([FromBody]LoginModel loginModel)
        {

            var result = await _userAuthenticationRepository.Login(loginModel);

            if(!result.IsSuccess)
                return StatusCode(401, result.Message);

            Response.Headers.Add("Authorization", "Bearer " + result.Token);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            if (_userRepository.DeleteUser(id))
                return Ok();
            return StatusCode(500, "Internal server error");
        }
    }
}
