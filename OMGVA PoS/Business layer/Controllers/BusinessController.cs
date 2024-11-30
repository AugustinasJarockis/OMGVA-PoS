using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Enums;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Data_layer.Repositories.Business_Management;
using OMGVA_PoS.Helper_modules.Utilities;
using System.IdentityModel.Tokens.Jwt;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [Route("business")]
    [ApiController]
    public class BusinessController(IBusinessRepository businessRepository) : Controller
    {
        private readonly IBusinessRepository _businessRepository = businessRepository;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBusinesses() {
            return Ok(JsonConvert.SerializeObject(_businessRepository.GetBusinesses()));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public IActionResult GetBusiness(long id) {
            return Ok(JsonConvert.SerializeObject(_businessRepository.GetBusinesses()));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateBusiness([FromBody]CreateBusinessRequest createBusinessRequest) {
            Business business = _businessRepository.CreateBusiness(createBusinessRequest); 
            //Check email field
            return Created($"/business/{business.Id}", business);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public IActionResult UpdateBusiness([FromBody] Business business, long id) {
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(id)) {
                return Forbid();
            }

            if (_businessRepository.UpdateBusiness(id, business))
                return Ok();
            else
                return NotFound();
        }
    }
}
