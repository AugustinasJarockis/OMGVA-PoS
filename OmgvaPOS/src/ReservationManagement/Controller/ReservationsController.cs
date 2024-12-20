﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.Exceptions;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ReservationManagement.DTOs;
using OmgvaPOS.ReservationManagement.Service;
using OmgvaPOS.SmsManagement;
using OmgvaPOS.UserManagement.Repository;
using static OmgvaPOS.Exceptions.ExceptionErrors;

namespace OmgvaPOS.ReservationManagement.Controller
{
    [ApiController]
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;
        private readonly IUserRepository _userRepository;

        public ReservationController(IReservationService reservationService, IUserRepository userRepository)
        {
            _reservationService = reservationService;
            _userRepository = userRepository;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReservationDto> UpdateReservation(long reservationId, [FromBody] UpdateReservationRequest updateRequest)
        {
            var businessId = JwtTokenHandler.GetTokenBusinessId(HttpContext.Request.Headers.Authorization);
            var reservation = _reservationService.Update(reservationId, updateRequest, businessId);
            return Ok(reservation);
        }

        [HttpDelete("{reservationId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ReservationDto>> GetBusinessReservations(long businessId)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, businessId))
                throw new ForbiddenException(GetForbiddenReservationErrorMessage(businessId));
            
            var reservations = _reservationService.GetAll();
            return Ok(reservations);
        }
        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ReservationDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<ReservationDto>> GetEmployeeReservations(long employeeId)
        {
            if (!AuthorizationHandler.CanManageUser(HttpContext.Request.Headers.Authorization, (long)_userRepository.GetUserNoException(employeeId)?.BusinessId, employeeId))
                throw new ForbiddenException("");

            var reservations = _reservationService.GetEmployeeReservations(employeeId);
            return Ok(reservations);
        }
    }
}