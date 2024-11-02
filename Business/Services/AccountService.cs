using Business.IServices;
using Business.Strategies;
using Core.IdentityModels;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationManagerJwtTokenGenerator _applicationManagerJwtTokenGenerator;
        private readonly ITenantUserJwtTokenGenerator _tenantUserJwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationManagerJwtTokenGenerator applicationManagerJwtTokenGenerator,
            ITenantUserJwtTokenGenerator tenantUserJwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationManagerJwtTokenGenerator = applicationManagerJwtTokenGenerator;
            _tenantUserJwtTokenGenerator = tenantUserJwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<IdentityResult> CreateApplicationManagerAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "ApplicationManager");
            if (!roleResult.Succeeded)
            {
                return roleResult;
            }

            return IdentityResult.Success;
        }

        public async Task<string> LoginApplicationManagerAsync(string email, string password)
        {
            var user = await FindByEmailAsync(email);
            if (user == null || !await _userManager.IsInRoleAsync(user, "ApplicationManager"))
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return await _applicationManagerJwtTokenGenerator.GenerateJwtToken(user);
            }

            return null;
        }

        public async Task<string> LoginAsync(string tenantId, string email, string password)
        {
            var user = await FindByEmailAsync(email);
            if (user == null || user.CompanyId != tenantId)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                return await _tenantUserJwtTokenGenerator.GenerateJwtToken(user);
            }

            return null;
        }

        public async Task<IdentityResult> RegisterCompanyManagerAsync(string firstName, string lastName, string email, string password, string companyName)
        {
            var existingCompany = await _unitOfWork.CompanyRepository.GetCompanyIdByCompanyName(companyName);
            if (existingCompany != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "This company name is already exist." });
            }

            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = companyName
            };

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Company = company,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "CompanyManager");
            if (!roleResult.Succeeded)
            {
                return roleResult;
            }

            var customerUser = new User
            {
                CompanyId = company.Id,
                ApplicationUserId = user.Id,
                FirstName = firstName,
                LastName = lastName
            };

            user.EmailConfirmed = false;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.UserRepository.AddAsync(customerUser);
            await _unitOfWork.CompleteAsync();
            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
