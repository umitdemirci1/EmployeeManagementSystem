using Business.DTOs;
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
        Task<List<Company>> GetApprovedCompanies();
        Task<List<Company>> GetPendingCompanies();
    }
}
