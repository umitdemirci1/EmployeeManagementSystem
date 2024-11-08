using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface ICompanyService
    {
        Task<bool> ValidateCompanyAsync(string companyName);
        Task<string> GetCompanyIdByCompanyName(string companyName);
        Task<bool> CreateCompanyAsync(CreateCompanyModel model);
        Task<List<ApprovedCompaniesResponse>> GetApprovedCompanies();
        Task<List<Company>> GetPendingCompanies();
    }
}
