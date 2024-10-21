using Core;
using Core.IdentityModels;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
               .HasOne(a => a.User)
               .WithOne(u => u.ApplicationUser)
               .HasForeignKey<User>(u => u.ApplicationUserId);

            var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

            foreach (var entityType in entityTypes)
            {
                if (typeof(EntityBase).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var filter = Expression.Lambda(Expression.Equal(
                        Expression.Property(parameter, nameof(EntityBase.IsDeleted)),
                        Expression.Constant(false)
                    ), parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }

                if (typeof(IMustHaveTenant).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var filter = Expression.Lambda(Expression.Equal(
                        Expression.Property(parameter, nameof(IMustHaveTenant.CompanyId)),
                        Expression.Constant(GetTenantId())
                    ), parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
                }
            }

            modelBuilder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted && u.CompanyId == GetTenantId());
            modelBuilder.Entity<ApplicationRole>().HasQueryFilter(r => !r.IsDeleted);
        }

        private string GetTenantId()
        {
            return _httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString();
        }
    }
}
