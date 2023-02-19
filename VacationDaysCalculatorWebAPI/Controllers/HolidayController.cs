using DomainModel.DtoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationDaysCalculatorWebAPI.Repositories;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HolidayController : ControllerBase
	{
		private readonly HolidayService _holidayService;
		private readonly HolidayRepository _holidayRepository;
		public HolidayController(HolidayService holidayService, HolidayRepository holidayRepository)
		{
			_holidayService = holidayService;
			_holidayRepository = holidayRepository;
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
		[HttpGet]
		public IActionResult GetHolidays()
		{
			try
			{
				return Ok(_holidayRepository.GetHolidays());
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
