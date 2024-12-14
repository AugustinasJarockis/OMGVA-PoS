using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.Exceptions;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Service;
using static OmgvaPOS.Exceptions.ExceptionErrors;

namespace OmgvaPOS.ReservationManagement.Controller
{
    [ApiController]
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDto> CreateReservation([FromBody] CreateReservationRequest createRequest)
        {
            var reservation = _reservationService.Create(createRequest);
            return CreatedAtAction(nameof(GetReservation), new { reservationId = reservation.Id }, reservation);
        }

        [HttpGet("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDto> GetReservation(long reservationId)
        {
            var reservation = _reservationService.GetById(reservationId);
            if (reservation == null)
                return NotFound(GetReservationNotFoundErrorResponse(reservationId));
            
            return Ok(reservation);
        }
        
        [HttpPut("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // TODO: if reservation status is done, don't let to update it for historical reasons
        public ActionResult<ReservationDto> UpdateReservation(long reservationId, [FromBody] UpdateReservationRequest updateRequest)
        {
            var reservation = _reservationService.Update(reservationId, updateRequest);
            return Ok(reservation);
        }

        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteReservation(long reservationId)
        {
            _reservationService.Delete(reservationId);
            return NoContent();
        }

        // TODO: take businessId not from url, but from token
        [HttpGet("business/{businessId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ReservationDto>> GetBusinessReservations(long businessId)
        {
            if (!JwtTokenHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization!, businessId))
                throw new ForbiddenException(GetForbiddenReservationErrorMessage(businessId));
            
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }

    }
}