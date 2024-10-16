using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserById(string userId);
    }
}
