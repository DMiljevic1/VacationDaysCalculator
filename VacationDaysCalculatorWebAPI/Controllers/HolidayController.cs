using DomainModel.DtoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HolidayController : ControllerBase
	{
		private readonly HolidayService _holidayService;
		public HolidayController(HolidayService holidayService)
		{
			_holidayService = holidayService;
		}
		[HttpPost]
		public IActionResult AddHolidays([FromBody] List<HolidayDetails> holidaysDetails)
		{
			try
			{
				_holidayService.AddHolidays(holidaysDetails);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
