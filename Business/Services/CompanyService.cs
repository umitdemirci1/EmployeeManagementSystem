using Business.IServices;
using Core.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CompanyService : ICompanySevice
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ValidateCompanyAsync(string companyName)
        {
            var company = await _unitOfWork.CompanyRepository.GetByNameAsync(companyName);
            if (company == null)
            {
                return false;
            }
            return true;
        }
    }
}
