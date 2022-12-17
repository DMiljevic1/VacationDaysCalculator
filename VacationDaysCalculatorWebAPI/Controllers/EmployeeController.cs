using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Claims;
using System.Security.Principal;
using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeRepository _userRepository;
        public EmployeeController(EmployeeRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId:int}")]
        [Authorize]
        public IActionResult GetEmployeeDetails(int userId)
        {
            try
            {
                return Ok(_userRepository.GetEmployeeDetails(userId));
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
                return Ok(_userRepository.GetEmployeeHistory(employeeId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddVacation([FromBody] VacationDays vacation)
        {
            if (vacation == null)
                return BadRequest();
            try
            {
                _userRepository.InsertVacation(vacation);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
