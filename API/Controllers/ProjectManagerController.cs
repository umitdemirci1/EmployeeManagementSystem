using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Business.IServices;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(Roles = "ApplicationManager")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectManagerController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public ProjectManagerController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("approved-companies")]
        public async Task<IActionResult> ApprovedCompaniesAsync()
        {
            var approvedCompanies = await _companyService.GetApprovedCompanies();
            if (approvedCompanies == null)
            {
                return Ok(new ApiResponse<object>(true, "No approved companies found"));
            }
            return Ok(new ApiResponse<object>(true, "Ok", approvedCompanies));
        }

        [HttpGet("pending-companies")]
        public async Task<IActionResult> PendingCompanies()
        {
            var pendingCompanies = await _companyService.GetPendingCompanies();
            if (pendingCompanies == null)
            {
                return Ok(new ApiResponse<object>(true, "No pending companies found"));
            }
            return Ok(new ApiResponse<object>(true, "Ok", pendingCompanies));
        }
    }
}