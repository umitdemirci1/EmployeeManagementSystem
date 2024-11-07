using Core.Models;
using DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Company> GetByNameAsync(string companyName)
        {
            return await _context.Companies.Where(c => c.Name == companyName).FirstOrDefaultAsync();
        }

        public async Task<string> GetCompanyIdByCompanyName(string companyName)
        {
            return await _context.Companies.Where(c => c.Name == companyName).Select(c => c.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Company>> GetApprovedCompanies()
        {
            return await _context.Companies.Where(c => c.IsApproved == true).ToListAsync();
        }

        public async Task<List<Company>> GetPendingCompanies()
        {
            return await _context.Companies.Where(c => c.IsApproved == false).ToListAsync();
        }
    }
}
