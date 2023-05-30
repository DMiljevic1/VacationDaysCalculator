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
	public class SickLeaveController : ControllerBase
	{
		private readonly SickLeaveService _sickLeaveService;
		private readonly SickLeaveRepository _sickLeaveRepository;
		public SickLeaveController(SickLeaveService sickLeaveService, SickLeaveRepository sickLeaveRepository)
		{
			_sickLeaveService = sickLeaveService;
			_sickLeaveRepository = sickLeaveRepository;
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

		[HttpGet("getMedicalCertificates/{sickLeaveId:int}")]
		[Authorize]
		public IActionResult GetMedicalCertificates(int sickLeaveId)
		{
			try
			{
				return Ok(_sickLeaveRepository.GetMedicalCertificates(sickLeaveId));
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}

		[HttpPost("addSickLeave")]
		[Authorize]
		public IActionResult AddSickLeave(SickLeave sickLeave)
		{
			try
			{
				_sickLeaveRepository.AddSickLeave(sickLeave);
				return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}


		[HttpPost("uploadMedicalCertificate")]
		[Authorize]
		public IActionResult UploadMedicalCertificate([FromBody] MedicalCertificate medicalCertificate)
		{
			try
			{
				_sickLeaveRepository.UploadMedicalCertificate(medicalCertificate);
			return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
	}
}
