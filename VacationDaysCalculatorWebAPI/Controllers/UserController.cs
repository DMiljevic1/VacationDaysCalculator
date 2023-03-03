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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
		private readonly UserRepository _userRepository;
		public UserController(UserService userService, UserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
        }

        [HttpGet("getPassword/{userId:int}")]
        [Authorize]
        public IActionResult GetUserPassword(int userId)
        {
            try
            {
                return Ok(_userService.GetUserPassword(userId));
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
               _userService.ChangePassword(password);
                return Ok();
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("getUsersViaScheduler")]
        [AllowAnonymous]
        public IActionResult GetUsers()
        {
			try
			{
				return Ok(_userRepository.GetUsers());
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
        [HttpPost("addUserViaScheduler")]
        [AllowAnonymous]
        public IActionResult AddUser([FromBody] User user)
        {
			try
			{
                _userRepository.AddUser(user);
                return Ok();
			}
			catch (System.Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError);
			}
		}
    }
}
