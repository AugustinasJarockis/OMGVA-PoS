using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            var scheduleWithAvailability = _scheduleService.GetEmployeeScheduleWithAvailability(employeeId, date);
            return Ok(scheduleWithAvailability);
        }
        [HttpGet("employee/{employeeId}")]
        public ActionResult<List<EmployeeSchedule>> GetAllSchedulesByEmployeeId(long employeeId)
        {
            var schedules = _scheduleService.GetAllSchedulesByEmployeeId(employeeId);
            return Ok(schedules);
        }
    }
}
