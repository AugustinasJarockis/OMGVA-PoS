using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Enums;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Data_layer.Repositories.Business_Management;
using OMGVA_PoS.Data_layer.Repositories.TaxManagement;
using OMGVA_PoS.Helper_modules.Utilities;
using System.IdentityModel.Tokens.Jwt;

namespace OMGVA_PoS.Business_layer.Controllers
{
    [Route("business")]
    [ApiController]
    public class BusinessController(IBusinessRepository businessRepository, ILogger<BusinessController> logger) : Controller
    {
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly ILogger<BusinessController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<List<Business>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBusinesses() {
            try {
                return Ok(JsonConvert.SerializeObject(_businessRepository.GetBusinesses()));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving all businesses.");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<Business>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusiness(long id) {
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token == null || token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(id)) {
                return Forbid();
            }

            try {
                Business business = _businessRepository.GetBusiness(id);

                if (business == null)
                    return NotFound();
                else
                    return Ok(JsonConvert.SerializeObject(business));
            }
            catch (Exception ex){
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving a business.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<Business>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBusiness([FromBody]CreateBusinessRequest createBusinessRequest) {
            if (!createBusinessRequest.Email.IsValidEmail())
                return StatusCode(400, "Email is not valid");
            if (!createBusinessRequest.Phone.IsValidPhone())
                return StatusCode(400, "Phone is not valid");

            try {
                Business business = _businessRepository.CreateBusiness(createBusinessRequest);
                if (business == null) {
                    _logger.LogError("An unexpected internal server error occured while creating the business.");
                    return StatusCode(500, "Internal server error.");
                }
                return Created($"/business/{business.Id}", business);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while creating the business.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateBusiness([FromBody] Business business, long id) {
            JwtSecurityToken token = JwtTokenHelper.GetJwtToken(HttpContext.Request.Headers.Authorization);
            if (token == null || token.UserRoleEquals(UserRole.Owner) && !token.UserBusinessEquals(id)) {
                return Forbid();
            }

            if (!business.Email.IsValidEmail())
                return StatusCode(400, "Email is not valid");
            if (!business.Phone.IsValidPhone())
                return StatusCode(400, "Phone is not valid");

            try {
                if (_businessRepository.UpdateBusiness(id, business))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the business.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
