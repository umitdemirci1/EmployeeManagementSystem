using Business.DTOs;
using Business.IServices;
using Core.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        
        [HttpPost("create-applicationmanager")]
        public async Task<IActionResult> CreateApplicationManager([FromBody] CreateApplicationManagerRequestModel request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email or password is required.");
            }

            var result = await _accountService.CreateApplicationManagerAsync(request.Email, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "ApplicationManager is succesfully created." });
        }

        [HttpPost("login/applicationmanager")]
        public async Task<IActionResult> LoginApplicationManager([FromBody] ApplicationManagerLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email or password is required.");
            }

            var token = await _accountService.LoginApplicationManagerAsync(request.Email, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid login credentials or authorization.");
            }

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.TenantId) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("TenantId, Email, and password are required.");
            }

            var token = await _accountService.LoginAsync(request.TenantId, request.Email, request.Password);
            if (token == null)
            {
                return Unauthorized("Invalid login credentials.");
            }

            return Ok(new { token });
        }
    }
}
