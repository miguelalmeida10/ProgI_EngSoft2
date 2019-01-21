using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalScheduling.Data
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] { "Administrator", "Nurse", "Doctor" };

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            using (IServiceScope serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!dbContext.Roles.Any())
                {
                    await dbContext.Database.MigrateAsync();

                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }

                    if (dbContext.Users.Where(u => u.UserName == "HospitalAdmin").FirstOrDefault() == null)
                    {
                        var admin = new IdentityUser() { UserName = "HospitalAdmin", Email = "HospitalAdmin@mail.com", EmailConfirmed = true };

                        Task create = userManager.CreateAsync(admin, "HAdmin");
                        create.Wait();
                        if (dbContext.Users.Where(u => u.UserName == "HospitalAdmin").FirstOrDefault() != null)
                        {
                            userManager.AddToRoleAsync(admin, "Administrator").Wait();
                        }
                    }
                }
            }
        }

        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            using (IServiceScope serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                if (!dbContext.Users.Where(u => u.UserName == "HospitalAdmin").Any())
                {
                    await dbContext.Database.MigrateAsync();

                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    var admin = new IdentityUser() { UserName = "HospitalAdmin", Email = "HospitalAdmin@mail.com", EmailConfirmed = true };

                    var result = await userManager.CreateAsync(admin, "HAdmin12");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Administrator");
                    }
                }
            }
        }
    }
}
