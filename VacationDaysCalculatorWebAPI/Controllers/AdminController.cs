﻿using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminRepository _adminRepository;
        public AdminController (AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("{userId:int}")]
        [Authorize]
        public IActionResult GetEmployeeDetails(int userId)
        {
            try
            {
                return Ok(_adminRepository.GetAdminDetails(userId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddUser([FromBody] UserDetails userDetails)
        {
            if (userDetails == null)
                return BadRequest();
            try
            {
                _adminRepository.AddUser(userDetails);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("approvedVacations")]
        [Authorize]
        public IActionResult GetApprovedVacations()
        {
            try
            {
                return Ok(_adminRepository.GetEmployeeVacationByStatus(VacationStatus.Approved));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
