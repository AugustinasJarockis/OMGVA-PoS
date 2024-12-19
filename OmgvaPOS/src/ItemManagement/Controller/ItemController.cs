using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Services;
using OmgvaPOS.UserManagement.Service;
using OmgvaPOS.DiscountManagement.Repository;

namespace OmgvaPOS.ItemManagement
{
    [Route("item")]
    [ApiController]
    public class ItemController(IItemService itemService, ITaxService taxService, IUserService userService, IDiscountRepository discountRepository, ILogger<ItemController> logger) : Controller
    {
        private readonly IItemService _itemService = itemService;
        private readonly IUserService _userService = userService;
        private readonly IDiscountRepository _discountRepository = discountRepository;
        private readonly ITaxService _taxService = taxService;
        private readonly ILogger<ItemController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<ItemDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBusinessItems() {
            long businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);

            return Ok(_itemService.GetItems(businessId));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItem(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemBusinessId(id)))
                return Forbid();

            var itemDTO = _itemService.GetItem(id);
            if (itemDTO == null)
                return NotFound();

            return Ok(itemDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateItem([FromBody] CreateItemRequest createItemRequest) { 
            long businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            
            ItemDTO item = _itemService.CreateItem(createItemRequest, businessId);
            return Created($"/item/{item.Id}", item);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateItem([FromBody] ItemDTO item, long id) {
            long itemBusinessId = _itemService.GetItemBusinessId(id);
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, itemBusinessId))
                return Forbid();

            item.Currency = item.Currency?.ToUpper();
            
            var returnItem = _itemService.UpdateItem(item, id);
            return Ok(returnItem);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemBusinessId(id)))
                return Forbid();

            _itemService.DeleteItem(id);
            return NoContent();
        }

        [HttpGet("{id}/taxes")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<TaxDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemTaxes(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemBusinessId(id)))
                return Forbid();

            List<TaxDto> itemTaxes = _itemService.GetItemTaxes(id);
            return Ok(itemTaxes);
        }

        [HttpPost("{id}/taxes")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ChangeItemTaxes([FromBody] ChangeItemTaxesRequest changeItemTaxesRequest, long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemBusinessId(id)))
                return Forbid();

            var allTaxIds = _taxService.GetAllTaxes().Select(t => t.Id);
            if (!(changeItemTaxesRequest.TaxesToRemoveIds.All(taxId => allTaxIds.Contains(taxId)) 
                && changeItemTaxesRequest.TaxesToAddIds.All(taxId => allTaxIds.Contains(taxId)))) {
                return NotFound("Not all specified taxes exist");
            }

            var returnItem = _itemService.ChangeItemTaxes(changeItemTaxesRequest, id);
            return Ok(returnItem);
        }
    }
}
