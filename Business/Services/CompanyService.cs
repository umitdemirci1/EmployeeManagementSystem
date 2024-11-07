using Business.DTOs;
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
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreateCompanyAsync(CreateCompanyModel model)
        {
            var newCompany = new Company
            {
                Name = model.Name
            };

            await _unitOfWork.CompanyRepository.AddAsync(newCompany);
            int changes = await _unitOfWork.CompleteAsync();
            if (changes > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<string> GetCompanyIdByCompanyName(string companyName)
        {
            var companyId = await _unitOfWork.CompanyRepository.GetCompanyIdByCompanyName(companyName);
            if (companyId == null)
            {
                return null;
            }
            return companyId;
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

        public async Task<List<Company>> GetApprovedCompanies()
        {
            var companies = await _unitOfWork.CompanyRepository.GetApprovedCompanies();
            if (companies == null)
            {
                return new List<Company>();
            }
            return companies;
        }
    }
}
