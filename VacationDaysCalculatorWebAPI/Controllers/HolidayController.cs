using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
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
		[AllowAnonymous]
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
		[AllowAnonymous]
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
		[HttpDelete("{holidayId:int}")]
		[Authorize]
		public IActionResult DeleteHoliday(int holidayId)
		{
			try
			{
				_holidayRepository.DeleteHoliday(holidayId);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
		[HttpPut("updateHoliday")]
		[Authorize]
		public IActionResult UpdateHoliday([FromBody] Holiday holiday)
		{
			try
			{
				_holidayService.UpdateHoliday(holiday);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
		[HttpPost("addHoliday")]
		[Authorize]
		public IActionResult AddHoliday([FromBody] Holiday holiday)
		{
			try
			{
				_holidayService.AddHoliday(holiday);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
