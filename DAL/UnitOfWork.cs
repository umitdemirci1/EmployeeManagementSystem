using Core;
using Core.Models;
using DAL.IRepositories;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private IUserRepository _userRepository;
        private ICompanyRepository _companyRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : EntityBase
        {
            if (_repositories.ContainsKey(typeof(T)))
            {
                return (IRepository<T>)_repositories[typeof(T)];
            }

            var repositoryType = typeof(T) switch
            {
                Type t when t == typeof(User) => typeof(UserRepository),
                Type t when t == typeof(Company) => typeof(CompanyRepository),
                //Other types will be added here
                _ => typeof(Repository<T>)
            };

            var repository = (IRepository<T>)Activator.CreateInstance(repositoryType, _context);
            _repositories.Add(typeof(T), repository);
            return repository;
        }

        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);
            }
        }

        public ICompanyRepository CompanyRepository
        {
            get
            {
                return _companyRepository ??= new CompanyRepository(_context);
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
