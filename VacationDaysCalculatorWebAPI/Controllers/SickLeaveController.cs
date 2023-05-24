using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SickLeaveController : ControllerBase
	{
		private readonly SickLeaveService _sickLeaveService;
		public SickLeaveController(SickLeaveService sickLeaveService)
		{
			_sickLeaveService = sickLeaveService;
		}

		[HttpPut("closeSickLeave")]
		[Authorize]
		public IActionResult CloseSickLeave([FromBody] SickLeave sickLeave)
		{
			try
			{
				_sickLeaveService.CloseSickLeave(sickLeave);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
