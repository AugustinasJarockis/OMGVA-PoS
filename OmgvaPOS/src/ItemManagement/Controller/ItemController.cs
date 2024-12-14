using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ItemManagement.DTOs;
using OmgvaPOS.ItemManagement.Models;
using OmgvaPOS.ItemManagement.Services;
using OmgvaPOS.ItemManagement.Mappers;
using OmgvaPOS.TaxManagement.Models;
using OmgvaPOS.TaxManagement.Services;
using OmgvaPOS.Validators;
using System.Net;
using OmgvaPOS.UserManagement.Service;
using OmgvaPOS.DiscountManagement.Repository;
using OmgvaPOS.Exceptions;

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
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            ItemDTO item = _itemService.GetItem(id);

            if (item == null)
                return NotFound();
            else
                return Ok(item);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<ItemDTO>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateItem([FromBody] CreateItemRequest createItemRequest) { //TODO: Validate here and in update method, that the employee, if exists, belongs to the correct business
            long businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization!);
            
            if (createItemRequest.UserId != null && _userService.GetUser((long)createItemRequest.UserId).BusinessId != businessId) //TODO: User id may be wrong here
                throw new NotFoundException("There is no such user that works in this business.");

            if (createItemRequest.DiscountId != null && _discountRepository.GetDiscount((long)createItemRequest.DiscountId).BusinessId != businessId) //TODO: Discount id may be wrong here
                throw new NotFoundException("There is no such discount available.");
            
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
            long itemBusinessId = _itemService.GetItemNoException(id).BusinessId;
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            item.Id = id;
            item.Currency = item.Currency?.ToUpper();

            if (item.UserId != null && _userService.GetUser((long)item.UserId).BusinessId != itemBusinessId) //TODO: User id may be wrong here
                throw new NotFoundException("There is no such user that works in this business.");

            if (item.DiscountId != null && _discountRepository.GetDiscount((long)item.DiscountId).BusinessId != itemBusinessId) //TODO: Discount id may be wrong here
                throw new NotFoundException("There is no such discount available.");

            
            var returnItem = _itemService.UpdateItem(item);
            if (returnItem != null) //TODO: Handle errors, handle possible null
                return Ok(returnItem);
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteItem(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
                return Forbid();

            if(_itemService.GetItem(id) == null) {
                return NotFound("Item not found.");
            }
            _itemService.DeleteItem(id); //TODO: Handle errors properly
            return NoContent();
        }

        [HttpGet("{id}/taxes")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<List<TaxDto>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] //TODO: Should be thrown if item does not exist.
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetItemTaxes(long id) {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId))
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
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, _itemService.GetItemNoException(id).BusinessId)) //TODO: Fix crash when inexistant business id is passed
                return Forbid();

            var allTaxIds = _taxService.GetAllTaxes().Select(t => t.Id);
            if (!(changeItemTaxesRequest.TaxesToRemoveIds.All(taxId => allTaxIds.Contains(taxId)) 
                && changeItemTaxesRequest.TaxesToAddIds.All(taxId => allTaxIds.Contains(taxId)))) {
                return NotFound("Not all specified taxes exist");
            }

            var returnItem = _itemService.ChangeItemTaxes(changeItemTaxesRequest, id);
            if (returnItem != null) //TODO: Handle errors, handle possible null
                return Ok(returnItem);
            else
                return NotFound();
        }
    }
}
