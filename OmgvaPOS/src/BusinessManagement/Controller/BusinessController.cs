using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.BusinessManagement.Repository;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.UserManagement.Enums;
using OmgvaPOS.Validators;

namespace OmgvaPOS.BusinessManagement.Controller
{
    [Route("business")]
    [ApiController]
    public class BusinessController(IBusinessRepository businessRepository, ILogger<BusinessController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IBusinessRepository _businessRepository = businessRepository;
        private readonly ILogger<BusinessController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<List<BusinessDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBusinesses() {
            try {
                return Ok(JsonConvert.SerializeObject(_businessRepository.GetBusinesses()));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving all businesses.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<BusinessDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusiness(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, id))
                return Forbid();

            try {
                BusinessDTO business = _businessRepository.GetBusiness(id);

                if (business == null)
                    return NotFound();
                else
                    return Ok(JsonConvert.SerializeObject(business));
            }
            catch (Exception ex){
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving a business.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        // [Authorize(Roles = "Admin")]
        [ProducesResponseType<BusinessDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateBusiness([FromBody]CreateBusinessRequest createBusinessRequest) {
            if (!createBusinessRequest.Email.IsValidEmail())
                return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid");
            if (!createBusinessRequest.Phone.IsValidPhone())
                return StatusCode((int)HttpStatusCode.BadRequest, "Phone is not valid");

            try {
                BusinessDTO business = _businessRepository.CreateBusiness(createBusinessRequest);
                if (business == null) {
                    _logger.LogError("An unexpected internal server error occured while creating the business.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
                }
                return Created($"/business/{business.Id}", business);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while creating the business.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
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
        public IActionResult UpdateBusiness([FromBody] BusinessDTO business, long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, id))
                return Forbid();

            if (!business.Email?.IsValidEmail() ?? false)
                return StatusCode((int)HttpStatusCode.BadRequest, "Email is not valid");
            if (!business.Phone?.IsValidPhone() ?? false)
                return StatusCode((int)HttpStatusCode.BadRequest, "Phone is not valid");

            try {
                if (_businessRepository.UpdateBusiness(id, business))
                    return Ok();
                else
                    return NotFound();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the business.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }
    }
}
