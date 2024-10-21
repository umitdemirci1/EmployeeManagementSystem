using API.Middlewares;
using Business.IServices;
using Business.Services;
using Core.IdentityModels;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanySevice, CompanyService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagementSystem")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoleManager<RoleManager<ApplicationRole>>()
    .AddUserManager<UserManager<ApplicationUser>>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

    await SeedRolesAsync(userManager, roleManager);
}

app.Run();

async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync("ApplicationManager"))
    {
        await roleManager.CreateAsync(new ApplicationRole { Name = "ApplicationManager" });
    }

    if (!await roleManager.RoleExistsAsync("CompanyManager"))
    {
        await roleManager.CreateAsync(new ApplicationRole { Name = "CompanyManager" });
    }

    if (!await roleManager.RoleExistsAsync("Employee"))
    {
        await roleManager.CreateAsync(new ApplicationRole { Name = "Employee" });
    }
}