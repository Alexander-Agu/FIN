using FIN.Dtos.UserDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Service.UserService;
using Microsoft.AspNetCore.Mvc;

namespace FIN.Controllers
{
    [Route("user")]
    [ApiController]

    public class UserController(IUserService service) : Controller
    {
        /*
         * Creates user account and saves information to the database
         */
        [HttpPost("/register")]
        public async Task<ActionResult<Dictionary<string, object>>> Register([FromBody] RegisterUserDto user)
        {
            Dictionary<string, object>? response = await service.RegisterUserAsync(user);

            if (response["result"].Equals(Result.Error)) return BadRequest(response);

            return Created(string.Empty, response);
        }


        /*
         * Confirms user account by enabling it
         */
        [HttpGet("confirm-email")]
        public async Task<ActionResult<Dictionary<string, object>>> ConfirmEmail(string token)
        {
            Dictionary<string, object>? response = await service.ConfirmEmailAsync(token);

            if (response["result"].Equals(Result.Error)) return BadRequest(response);

            return Ok(response);
        }
    }
}
