using Core;
using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : EntityBase;
        Task<int> CompleteAsync();
        IUserRepository UserRepository { get; }
        ICompanyRepository CompanyRepository { get; }
    }
}
