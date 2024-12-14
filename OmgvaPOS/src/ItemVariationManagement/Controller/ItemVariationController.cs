using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.ItemVariationManagement.Services;
using OmgvaPOS.ItemManagement;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemManagement.Services;

namespace OmgvaPOS.ItemVariationManagement
{
    [ApiController]
    [Route("item-variation")]
    public class ItemVariationController(IItemVariationService itemVariationService, IItemService itemService, ILogger<ItemController> logger) : Controller
    {
        private readonly IItemVariationService _itemVariationService = itemVariationService;
        private readonly IItemService _itemService = itemService;
        private readonly ILogger<ItemController> _logger = logger;

        [HttpGet("item/{itemId}")] //TODO: Check for potential additional errors
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<ItemVariationDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllItemsVariations(long itemId) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(itemId).BusinessId))
                return Forbid();

            try {
                return Ok(JsonConvert.SerializeObject(_itemVariationService.GetItemVariations(itemId)));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving all item variations.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemVariation(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemVariationService.GetItemVariationBusinessNoException(id)))
                return Forbid();

            try {
                ItemVariationDTO itemVariation = _itemVariationService.GetItemVariation(id);

                if (itemVariation == null)
                    return NotFound();
                else
                    return Ok(JsonConvert.SerializeObject(itemVariation));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while retrieving an item variation.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpPost("{itemId}")] //TODO: Check for potential additional errors
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateItemVariation([FromBody] ItemVariationCreationRequest itemVariationCreationRequest, long itemId) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(itemId).BusinessId))
                return Forbid();

            try {
                ItemVariationDTO itemVariationDTO = _itemVariationService.CreateItemVariation(itemVariationCreationRequest, itemId);
                if (itemVariationDTO == null) {
                    _logger.LogError("An unexpected internal server error occured while creating the item variation.");
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
                }
                return Created($"/item/{itemVariationDTO.Id}", itemVariationDTO);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while creating the item variation.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpPatch("{id}")] //Check other status code possibility
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateItemVariation([FromBody] ItemVariationUpdateRequest itemVariationUpdateRequest, long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemVariationService.GetItemVariationBusinessNoException(id)))
                return Forbid();

            try {
                var returnedItemVariation = _itemVariationService.UpdateItemVariation(itemVariationUpdateRequest, id);
                if (returnedItemVariation != null) //TODO: Handle errors, possible null value
                    return Ok(returnedItemVariation);
                else
                    return NotFound();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while updating the item variation.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        [HttpDelete("{id}")] //TODO: Consider error codes
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemVariationService.GetItemVariationBusinessNoException(id)))
                return Forbid();

            try {
                if (_itemVariationService.GetItemVariation(id) == null) {
                    return NotFound("Item variation not found.");
                }
                _itemVariationService.DeleteItemVariation(id); //TODO: Handle errors properly
                return NoContent();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "An unexpected internal server error occured while deleting item variation.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }
    }
}
