using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.Exceptions;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Service;
using static OmgvaPOS.Exceptions.ExceptionMessages;

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
        public ActionResult<ReservationDto> CreateReservation([FromBody] CreateReservationDto createDto)
        {
            var reservation = _reservationService.Create(createDto);
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
                throw new NotFoundException(ReservationNotFoundMessage(reservationId));
            
            return Ok(reservation);
        }
        
        [HttpPut("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDto> UpdateReservation(long reservationId, [FromBody] UpdateReservationDto updateDto)
        {
            var reservation = _reservationService.Update(reservationId, updateDto);
            return Ok(reservation);
        }

        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteReservation(long reservationId)
        {
            _reservationService.Delete(reservationId);
            return NoContent();
        }

        [HttpGet("business/{businessId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        // TODO: add forbidden once we link reservation to business
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ReservationDto>> GetBusinessReservations(long businessId)
        {
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }

    }
}