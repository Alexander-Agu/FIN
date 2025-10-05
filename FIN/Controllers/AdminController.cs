using FIN.Dtos.AdminDtos;
using FIN.Entities;
using FIN.Enums;
using FIN.Service.AdminService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIN.Controllers
{
    [Route("admin")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        // Register's admin
        [HttpPost("register")]
        public async Task<ActionResult<Dictionary<string, object>>> Register([FromBody] CreateAdminDto admin)
        {
            Dictionary<string, object>? response = await adminService.RegisterAsync(admin);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Created(string.Empty, response);
        }


        // Activates admin's account
        [HttpGet("confirm-email")]
        public async Task<ActionResult<Dictionary<string, object>>> ConfirmAccount([FromQuery] string otp)
        {
            Dictionary<string, object> response = await adminService.ConfirmEmailAsync(otp);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Resends confirmation email
        [HttpGet("Resend-confirm-email")]
        public async Task<ActionResult<Dictionary<string, object>>> ResendConfirmationEmail([FromQuery] string email)
        {
            Dictionary<string, object>? response = await adminService.ResendVarificarionEmailAsync(email);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Logs admin into their account
        [HttpPost("login")]
        public async Task<ActionResult<Dictionary<string, object>>> Login([FromBody] LoginDto login)
        {
            Dictionary<string, object>? response = await adminService.LoginAsync(login);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Fetches admin information
        [HttpGet("get-admin/{adminId}")]
        public async Task<ActionResult<Dictionary<string, object>>> GetAdmin(int adminId)
        {
            Dictionary<string, object>? response = await adminService.GetAdminAsync(adminId);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Update admin data
        [HttpPut("update-profile/{adminId}")]
        public async Task<ActionResult<Dictionary<string, object>>> UpdateProfile(int adminId, [FromBody] UpdateAdminProfileDto profile)
        {
            Dictionary<string, object>? response = await adminService.UpdateAdminProfile(adminId, profile);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Update email
        [HttpPut("update-email/{adminId}")]
        public async Task<ActionResult<Dictionary<string, object>>> UpdateEmail(int adminId, [FromBody] UpdateEmailDto email)
        {
            Dictionary<string, object>? response = await adminService.UpdateEmailAsync(adminId, email);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // Sends forgoten email password
        [HttpGet("forgoten-password-email")]
        public async Task<ActionResult<Dictionary<string, object>>> SendForgotenPasswordEmail([FromQuery] string email)
        {
            Dictionary<string, object>? response = await adminService.SendUpdatePasswordEmailAsync(email);

            if (response["result"].Equals(Result.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
