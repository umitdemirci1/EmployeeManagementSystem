using Core.Models;
using DAL;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

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
            var path = context.Request.Path.Value;

            if (path.Contains("/api/account/login") || path.Contains("/api/account/create-applicationmanager"))
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                var tenantId = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "tenantId")?.Value;
                var role = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;

                if (role == "ApplicationManager")
                {
                    await _next(context);
                    return;
                }

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
            }

            await _next(context);
        }

        private async Task<Company> LoadTenantAsync(AppDbContext dbContext, string tenantId)
        {
            return await dbContext.Companies.FirstOrDefaultAsync(c => c.Id == tenantId);
        }
    }
}
