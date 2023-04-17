using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using System.Security.Principal;
using VacationDaysCalculatorWebAPI.Repositories;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        private readonly EmailService _emailService;
		public EmployeeController(EmployeeService employeeService, EmailService emailService)
        {
            _employeeService = employeeService;
            _emailService = emailService;
        }

        [HttpGet("{userId:int}")]
        [Authorize]
        public IActionResult GetEmployeeDetails(int userId)
        {
            try
            {
                return Ok(_employeeService.GetEmployeeDetails(userId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("employeeHistory/{employeeId:int}")]
        [Authorize]
        public IActionResult GetEmployeeHistory(int employeeId)
        {
            try
            {
                return Ok(_employeeService.GetEmployeeHistory(employeeId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddVacation([FromBody] Vacation vacation)
        {
            if (vacation == null)
                return BadRequest();
            try
            {
                _employeeService.InsertVacation(vacation);
                _emailService.SendVacationRequestMail();
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{vacationId:int}")]
        [Authorize]
        public IActionResult DeleteVacationRequestAndRestoreRemainingVacation(int vacationId)
        {
            try
            {
                _employeeService.DeleteVacationRequestAndRestoreRemainingVacation(vacationId);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("updateVacationStatus")]
        [Authorize]
        public IActionResult UpdateEmployeeVacationStatus([FromBody] Vacation vacation)
        {
            try
            {
                _employeeService.UpdateEmployeeVacationStatus(vacation);
                _emailService.SendVacationResponseMail(vacation.UserId, vacation.Status);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("updateVacationStatusViaScheduler")]
        [AllowAnonymous]
        public IActionResult UpdateEmployeeVacationStatus([FromBody] DateTime currentDate)
        {
            try
            {
                _employeeService.SetVacationStatus(currentDate);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("updateRemainingVacation")]
        [AllowAnonymous]
        public IActionResult UpdateRemainingVacation()
        {
            try
            {
                _employeeService.SetRemainingVacationOnFirstDayOfYear();
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult CalculateTotalVacationForGivenPeriodAsync([FromBody] List<DateTime> vacation)
        {
            try
            {
                var vacationFrom = vacation[0];
                var vacationTo = vacation[1];
                return Ok(_employeeService.CalculateTotalVacationForGivenPeriod(vacationFrom, vacationTo));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
	}
}
