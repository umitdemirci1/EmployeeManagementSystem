using Core.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
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

        public static async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            var email = configuration["SeedData:AdminEmail"];
            var password = configuration["SeedData:AdminPassword"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Email or AdminPassword is missing in the configuration.");
                return;
            }

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    Console.WriteLine("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    return;
                }

                var roleResult = await userManager.AddToRoleAsync(user, "ApplicationManager");
                if (!roleResult.Succeeded)
                {
                    Console.WriteLine("Failed to add user to role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                Console.WriteLine("User already exists.");
            }
        }
    }
}
