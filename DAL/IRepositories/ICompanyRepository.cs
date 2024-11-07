using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> GetByNameAsync(string companyName);
        Task<string> GetCompanyIdByCompanyName(string companyName);
        Task<List<Company>> GetApprovedCompanies();
    }
}
