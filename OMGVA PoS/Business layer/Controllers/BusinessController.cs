using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Enums;
using OMGVA_PoS.Data_layer.Models;
using OMGVA_PoS.Data_layer.Repositories.Business_Management;

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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateBusiness([FromBody]CreateBusinessRequest createBusinessRequest) {
            createBusinessRequest.Owner.Role = UserRole.Owner;
            Business business = _businessRepository.CreateBusiness(createBusinessRequest); 
            return Created($"/business/{business.Id}", business);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public IActionResult UpdateBusiness([FromBody]UpdateBusinessRequest updateBusinessRequest {
            //TODO: check if business belongs to the owner
            //TODO: update business
            //TODO: throw error codes
        }
    }
}
