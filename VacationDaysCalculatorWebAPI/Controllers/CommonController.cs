using DomainModel.DtoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VacationDaysCalculatorWebAPI.Repositories;
using VacationDaysCalculatorWebAPI.Services;

namespace VacationDaysCalculatorWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly CommonService _commonService;
        public CommonController(CommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("getPassword/{userId:int}")]
        [Authorize]
        public IActionResult GetUserPassword(int userId)
        {
            try
            {
                return Ok(_commonService.GetUserPassword(userId));
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("changePassword")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] Password password)
        {
            try
            {
               _commonService.ChangePassword(password);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
