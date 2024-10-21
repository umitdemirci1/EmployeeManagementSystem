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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddUserAsync(User user)
        {
            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task CompleteAsync()
        {
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            await _unitOfWork.Repository<User>().DeleteAsync(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IQueryable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.Repository<User>().GetAllAsync();
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _unitOfWork.Repository<User>().GetByIdAsync(userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _unitOfWork.Repository<User>().UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }
    }
}
