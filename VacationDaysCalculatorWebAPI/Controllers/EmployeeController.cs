using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using System.Security.Principal;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeRepository;
        public EmployeeController(EmployeeService employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("{userId:int}")]
        [Authorize]
        public IActionResult GetEmployeeDetails(int userId)
        {
            try
            {
                return Ok(_employeeRepository.GetEmployeeDetails(userId));
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
                return Ok(_employeeRepository.GetEmployeeHistory(employeeId));
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
                _employeeRepository.InsertVacation(vacation);
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
                _employeeRepository.DeleteVacationRequestAndRestoreRemainingVacation(vacationId);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("approveVacation")]
        [Authorize]
        public IActionResult ApproveVacation([FromBody] Vacation vacation)
        {
            try
            {
                _employeeRepository.ApproveVacation(vacation);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("cancelVacation")]
        [Authorize]
        public IActionResult CancelVacation([FromBody] Vacation vacation)
        {
            try
            {
                _employeeRepository.CancelVacation(vacation);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("updateVacationStatus")]
        [AllowAnonymous]
        public IActionResult UpdateEmployeeVacationStatus([FromBody] DateTime currentDate)
        {
            try
            {
                _employeeRepository.SetVacationStatus(currentDate);
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
                _employeeRepository.SetRemainingVacationOnFirstDayOfYear();
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
                return Ok(_employeeRepository.CalculateTotalVacationForGivenPeriod(vacationFrom, vacationTo));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
