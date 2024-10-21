using Core.Models;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace API.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();

            if (!string.IsNullOrEmpty(tenantId))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var tenant = await LoadTenantAsync(dbContext, tenantId);
                    if (tenant != null)
                    {
                        context.Items["TenantId"] = tenant.Id;
                    }
                }
            }

            await _next(context);
        }

        private async Task<Company> LoadTenantAsync(AppDbContext dbContext, string tenantId)
        {
            return await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == tenantId);
        }
    }
}
