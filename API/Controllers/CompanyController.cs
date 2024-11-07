using Business.DTOs;
using Business.IServices;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companySevice)
        {
            _companyService = companySevice;
        }

        [HttpGet("get-company-id")]
        public async Task<IActionResult> GetCompanyIdByCompanyName(string companyName)
        {
            if (string.IsNullOrEmpty(companyName))
            {
                return BadRequest("Company name is required");
            }

            var companyId = await _companyService.GetCompanyIdByCompanyName(companyName);
            if (companyId == null)
            {
                return BadRequest("Company name is invalid");
            }

            return Ok(new { companyId });
        }

        [Authorize(Roles = "ApplicationManager")]
        [HttpPost("create-company")]
        public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                return BadRequest("Company name is required");
            }

            var company = await _companyService.GetCompanyIdByCompanyName(model.Name);
            if (company != null)
            {
                return BadRequest("Company name already exists");
            }

            var result = await _companyService.CreateCompanyAsync(model);
            if (result)
            {
                return Ok("Company created successfully");
            }
            else
            {
                return BadRequest("Company creation failed");
            }
        }
    }
}
