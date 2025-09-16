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
        [HttpPost("register")]
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


        /*
         * Resends varification email
         */
        [HttpGet("resend-email")]
        public async Task<ActionResult<Dictionary<string, object>>> Resendvarification([FromQuery] string email)
        {
            Dictionary<string, object>? response = await service.ResendVarificationMailAsync(email);

            if (response["result"].Equals(Result.Error)) return BadRequest(response);

            return Ok(response);
        }


        /*
         * Login user
         */
        [HttpPost("login")]
        public async Task<ActionResult<Dictionary<string, object>>> Login([FromBody] LoginDto login)
        {
            Dictionary<string, object>? response = await service.LoginAsync(login);

            if (response["result"].Equals(Result.Error)) return BadRequest(response);

            return Ok(response);
        }


        /*
         * Get user data
         */
        [HttpGet("get-user/{userId}")]
        public async Task<ActionResult<Dictionary<string, object>>> GetUser(int userId)
        {
            Dictionary<string, object>? response = await service.GetUserAsync(userId);

            if (response["result"].Equals(Result.Error)) return BadRequest(response);

            return Ok(response);
        }
    }
}
