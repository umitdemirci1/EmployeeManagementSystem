using Business.IServices;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanySevice _companyService;

        public CompanyController(ICompanySevice companySevice)
        {
            _companyService = companySevice;
        }

        [HttpGet]
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCompany([FromBody] string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                return BadRequest("Company name is required");
            }

            var result = await _companyService.ValidateCompanyAsync(companyName);
            if (result)
            {
                return Ok("Company name is valid");
            }
            else
            {
                return BadRequest("Company name is invalid");
            }
        }
    }
}
