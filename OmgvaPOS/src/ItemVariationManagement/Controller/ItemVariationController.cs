using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.ItemVariationManagement.Services;
using Microsoft.AspNetCore.Authorization;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ItemVariationManagement.DTOs;
using OmgvaPOS.ItemManagement.Services;

namespace OmgvaPOS.ItemVariationManagement
{
    [ApiController]
    [Route("item-variation")]
    public class ItemVariationController(IItemVariationService itemVariationService, IItemService itemService) : Controller
    {
        private readonly IItemVariationService _itemVariationService = itemVariationService;
        private readonly IItemService _itemService = itemService;

        [HttpGet("item/{itemId}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<ItemVariationDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllItemsVariations(long itemId)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _itemService.GetItemBusinessId(itemId)))
                return Forbid();

            return Ok(_itemVariationService.GetItemVariations(itemId));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemVariation(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _itemVariationService.GetItemVariationBusinessId(id)))
                return Forbid();

            var itemVariation = _itemVariationService.GetItemVariation(id);

            if (itemVariation == null)
                return NotFound();
            else
                return Ok(itemVariation);
        }

        [HttpPost("{itemId}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateItemVariation([FromBody] ItemVariationCreationRequest itemVariationCreationRequest, long itemId) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _itemService.GetItemBusinessId(itemId)))
                return Forbid();

            var itemVariationDTO = _itemVariationService.CreateItemVariation(itemVariationCreationRequest, itemId);
            return Created($"/item/{itemVariationDTO.Id}", itemVariationDTO);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemVariationDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateItemVariation([FromBody] ItemVariationUpdateRequest itemVariationUpdateRequest, long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _itemVariationService.GetItemVariationBusinessId(id)))
                return Forbid();

            var returnedItemVariation = _itemVariationService.UpdateItemVariation(itemVariationUpdateRequest, id);
            return Ok(returnedItemVariation);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _itemVariationService.GetItemVariationBusinessId(id)))
                return Forbid();

            _itemVariationService.DeleteItemVariation(id);
            return NoContent();
        }
    }
}
