using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.BusinessManagement.DTOs;
using OmgvaPOS.BusinessManagement.Services;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.Validators;

namespace OmgvaPOS.BusinessManagement.Controller
{
    [Route("business")]
    [ApiController]
    public class BusinessController(IBusinessService businessService, ILogger<BusinessController> logger) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IBusinessService _businessService = businessService;
        private readonly ILogger<BusinessController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType<List<BusinessDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBusinesses()
        {
            var businessDTOs = _businessService.GetBusinesses();
            return Ok(businessDTOs);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType<BusinessDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetBusiness(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, id))
                return Forbid();

            var business = _businessService.GetBusiness(id);

            if (business == null)
                return NotFound();
            
            return Ok(business);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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

            var business = _businessService.CreateBusiness(createBusinessRequest);
            return Created($"/business/{business.Id}", business);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateBusiness([FromBody] BusinessDTO businessDTO, long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, id))
                return Forbid();
            
            if (_businessService.UpdateBusiness(businessDTO, id))
                return Ok();
            
            return NotFound();
        }
    }
}
