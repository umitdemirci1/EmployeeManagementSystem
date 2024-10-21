using Business.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpPost("login")]
        //public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        //{
        //    var token = await _accountService.LoginAsync(model.TenantId, model.Email, model.Password);
        //    if (token == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(token);
        //}
    }
}
