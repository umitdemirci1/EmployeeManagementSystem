using Core.IdentityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.IServices
{
    public interface IAccountService
    {
        Task<string> LoginAsync(string tenantId, string email, string password);
        Task<string> LoginApplicationManagerAsync(string email, string password);
        Task<(IdentityResult Result, IEnumerable<string> Errors)> CreateApplicationManagerAsync(string email, string password);
        Task<(IdentityResult Result, IEnumerable<string> Errors)> RegisterCompanyManagerAsync(string firstName, string lastName, string email, string password, string companyName);
        Task<ApplicationUser> FindByEmailAsync(string email);
    }
}
