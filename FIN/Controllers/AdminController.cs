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
        [HttpPost("/register")]
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
    }
}
