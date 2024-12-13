using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.src.ItemManagement.Mappers;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Services;
using OmgvaPOS.Validators;
using System.Net;

namespace OmgvaPOS.ItemManagement
{
    [Route("item")]
    [ApiController]
    public class ItemController(IItemService itemService, ITaxService taxService, ILogger<ItemController> logger) : Controller
    {
        private readonly IItemService _itemService = itemService;
        private readonly ITaxService _taxService = taxService;
        private readonly ILogger<ItemController> _logger = logger;

        [HttpGet]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<ItemDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllBusinessItems() {
            long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            if (businessId == null)
                return Forbid();

            try {
                return Ok(_itemService.GetItems((long)businessId));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving all items.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItem(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            try {
                ItemDTO item = _itemService.GetItem(id);

                if (item == null)
                    return NotFound();
                else
                    return Ok(item);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving an item.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateItem([FromBody] CreateItemRequest createItemRequest) { //TODO: Validate here and in update method, that the employee, if exists, belongs to the correct business
            long? businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            if (businessId == null)
                return Forbid();

            createItemRequest.Currency = createItemRequest.Currency.ToUpper();
            if (!createItemRequest.Currency.IsValidCurrency())
                return StatusCode((int)HttpStatusCode.BadRequest, "Currency is not valid");

            if (createItemRequest.InventoryQuantity < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "Inventory quantity can not be negative");

            if (createItemRequest.Price < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "Price can not be negative");

            try {
                Item newitem = createItemRequest.ToItem((long)businessId);
                ItemDTO item = _itemService.CreateItem(newitem);
                if (item == null) {
                    _logger.LogError("An unexpected internal server error occured while creating the item.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
                }
                return Created($"/item/{item.Id}", item);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while creating the item.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
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
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            item.Id = id;
            item.Currency = item.Currency.ToUpper();

            if (!item.Currency.IsValidCurrency())
                return StatusCode((int)HttpStatusCode.BadRequest, "Currency is not valid");

            if (item.InventoryQuantity < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "Inventory quantity can not be negative");

            if (item.Price < 0)
                return StatusCode((int)HttpStatusCode.BadRequest, "Price can not be negative");

            try {
                var returnItem = _itemService.UpdateItem(item);
                if (returnItem != null) //TODO: Handle errors, handle possible null
                    return Ok(returnItem);
                else
                    return NotFound();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the item.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            try {
                if(_itemService.GetItem(id) == null) {
                    return NotFound("Item not found.");
                }
                _itemService.DeleteItem(id); //TODO: Handle errors properly
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while deleting item.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("{id}/taxes")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<TaxDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //TODO: Should be thrown if item does not exist.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemTaxes(long id) {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            try {
                List<TaxDto> itemTaxes = _itemService.GetItemTaxes(id);
                return Ok(itemTaxes);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving item taxes.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
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
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId)) //TODO: Fix crash when inexistant business id is passed
                return Forbid();

            var allTaxIds = _taxService.GetAllTaxes().Select(t => t.Id);
            if (!(changeItemTaxesRequest.TaxesToRemoveIds.All(taxId => allTaxIds.Contains(taxId)) 
                && changeItemTaxesRequest.TaxesToAddIds.All(taxId => allTaxIds.Contains(taxId)))) {
                return NotFound("Not all specified taxes exist");
            }

            try {
                var returnItem = _itemService.ChangeItemTaxes(changeItemTaxesRequest, id);
                if (returnItem != null) //TODO: Handle errors, handle possible null
                    return Ok(returnItem);
                else
                    return NotFound();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while changing item taxes.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }
    }
}
