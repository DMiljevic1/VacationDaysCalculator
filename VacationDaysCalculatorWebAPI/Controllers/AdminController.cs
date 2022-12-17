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
    }
}
