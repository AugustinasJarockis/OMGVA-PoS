using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmgvaPOS.BusinessManagement.Models;
using OmgvaPOS.HelperUtils;
using OmgvaPOS.ScheduleManagement.DTOs;
using OmgvaPOS.ScheduleManagement.Models;
using OmgvaPOS.ScheduleManagement.Service;

namespace OmgvaPOS.ScheduleManagement.Controller
{
    [ApiController]
    [Route("schedules")]
    public class ScheduleController(IScheduleService scheduleService) : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IScheduleService _scheduleService = scheduleService;

        [HttpPost]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<EmployeeSchedule>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateEmployeeSchedule([FromBody] CreateEmployeeScheduleRequest request)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployee(request.EmployeeId)))
                return Forbid();

            var schedule = _scheduleService.CreateEmployeeSchedule(request);
            return CreatedAtAction(nameof(GetEmployeeSchedule), new { id = schedule.Id }, schedule);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteEmployeeSchedule(long id)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployeeSchedule(id)))
                return Forbid();

            _scheduleService.DeleteEmployeeSchedule(id);
            return NoContent();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<EmployeeSchedule>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployeeSchedule(long id, [FromBody] UpdateEmployeeScheduleRequest request)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployeeSchedule(id)))
                return Forbid();

            var updatedSchedule = _scheduleService.UpdateEmployeeSchedule(id, request);
            return Ok(updatedSchedule);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<ScheduleWithAvailability>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeSchedule(long id)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployeeSchedule(id)))
                return Forbid();

            var scheduleWithAvailability = _scheduleService.GetEmployeeSchedule(id);
            return Ok(scheduleWithAvailability);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<EmployeeSchedulesWithAvailability>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeesSchedulesByItemAndDate([FromQuery] long itemId, [FromQuery] DateOnly date)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromItem(itemId)))
                return Forbid();

            var schedules = _scheduleService.GetEmployeesSchedulesByItemAndDate(itemId, date);
            return Ok(schedules);
        }

        [HttpGet("employee")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<EmployeeSchedulesWithAvailability>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetEmployeeScheduleWithAvailability([FromQuery] long employeeId, [FromQuery] DateOnly date)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployee(employeeId)))
                return Forbid();

            var scheduleWithAvailability = _scheduleService.GetEmployeeScheduleWithAvailability(employeeId, date);
            return Ok(scheduleWithAvailability);
        }
        [HttpGet("employee/{employeeId}")]
        [Authorize(Roles = "Admin,Owner,Employee")]
        [ProducesResponseType<List<EmployeeSchedule>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllSchedulesByEmployeeId(long employeeId)
        {
            if (!AuthorizationHandler.CanManageBusiness(HttpContext.Request.Headers.Authorization, _scheduleService.GetBusinessIdFromEmployee(employeeId)))
                return Forbid();

            var schedules = _scheduleService.GetAllSchedulesByEmployeeId(employeeId);
            return Ok(schedules);
        }
    }
}
