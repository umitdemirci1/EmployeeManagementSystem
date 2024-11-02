using Business.DTOs;
using Business.IServices;
using Core.IdentityModels;
using Core.Models;
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
                return BadRequest(new ApiResponse<object>(false, "Email or password is required."));
            }

            var (result, errors) = await _accountService.CreateApplicationManagerAsync(request.Email, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<object>(false, "Failed to create ApplicationManager.", null, errors));
            }

            return Ok(new ApiResponse<object>(true, "ApplicationManager is successfully created."));
        }

        [HttpPost("login/applicationmanager")]
        public async Task<IActionResult> LoginApplicationManager([FromBody] ApplicationManagerLoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponse<object>(false, "Email or password is required."));
            }

            var token = await _accountService.LoginApplicationManagerAsync(request.Email, request.Password);
            if (token == null)
            {
                return Unauthorized(new ApiResponse<object>(false, "Invalid login credentials or authorization."));
            }

            return Ok(new ApiResponse<string>(true, "Login successful.", token));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.TenantId) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ApiResponse<object>(false, "TenantId, Email, and password are required."));
            }

            var token = await _accountService.LoginAsync(request.TenantId, request.Email, request.Password);
            if (token == null)
            {
                return Unauthorized(new ApiResponse<object>(false, "Invalid login credentials."));
            }

            return Ok(new ApiResponse<string>(true, "Login successful.", token));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<object>(false, "Invalid model state.", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            var (result, errors) = await _accountService.RegisterCompanyManagerAsync(request.FistName, request.LastName, request.Email, request.Password, request.CompanyName);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse<object>(false, "Registration failed.", null, errors));
            }

            // TODO: Notify application managers
            var user = await _accountService.FindByEmailAsync(request.Email);
            //await _notificationService.NotifyApplicationManagersAsync(user);

            return Ok(new ApiResponse<object>(true, "Registration successful, awaiting confirmation."));
        }
    }
}
