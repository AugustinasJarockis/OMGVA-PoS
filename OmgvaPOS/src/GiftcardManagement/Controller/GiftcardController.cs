using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.GiftcardManagement.DTOs;
using OmgvaPOS.GiftcardManagement.Models;
using OmgvaPOS.GiftcardManagement.Service;
using OmgvaPOS.HelperUtils;
using System.Net;

namespace src.GiftcardManagement.Controller
{
    [ApiController]
    [Route("giftcard")]
    public class GiftcardController(IGiftcardService giftcardService) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IGiftcardService _giftcardService = giftcardService;

        [HttpPost]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<Giftcard>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Create([FromBody] GiftcardDTO giftcardRequest)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, giftcardRequest.BusinessId))
                return Forbid();

            try
            {
                var giftcard = _giftcardService.CreateGiftcard(giftcardRequest);
                return Ok(giftcard);
            }
            catch (ApplicationException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Value should be positive.");
            }
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType<Giftcard>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetGiftcard(long id)
        {
            var giftcard = _giftcardService.GetGiftcard(id);

            if (giftcard == null)
                return NotFound();

            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, giftcard.BusinessId))
                return Forbid();

            return Ok(giftcard);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Owner")]
        [ProducesResponseType<Giftcard>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetGiftcards()
        {
            long businessId = (long)JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);

            var giftcards = _giftcardService.GetGiftcards(businessId);

            return Ok(giftcards);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin, Owner, Employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public IActionResult UpdateGiftcard([FromBody] GiftcardUpdateRequest giftcardRequest)
        {
            try
            {
                var giftcard = _giftcardService.GetGiftcard(giftcardRequest.Code);

                if (giftcard == null)
                    return UnprocessableEntity();

                if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, giftcard.BusinessId))
                    return Forbid();

                _giftcardService.UpdateGiftcard(giftcardRequest);
                return Ok();
            }
            catch (ApplicationException)
            {
                return UnprocessableEntity();
            }
        }
    }
}
