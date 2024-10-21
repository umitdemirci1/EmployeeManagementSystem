using Core.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class LoginService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> LoginAsync(string email, string password, int companyId)
        {
            //var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == email && u.CompanyId == companyId);
            //if (user != null && VerifyPassword(password, user.PasswordHash))
            //{
            //    // Kullanıcı doğrulandı
            //    return user;
            //}

            return null;
        }
    }

}
