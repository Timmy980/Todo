using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace Todo.Models
{
    public enum RoleNames
    {
        Admin,
        User
    }
    public class SeedData
    {
        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(RoleNames)))
            {
                if (!await roleManager.RoleExistsAsync(role.ToString()))
                {
                    var newRole = new IdentityRole { Name = role.ToString() };
                    await roleManager.CreateAsync(newRole);
                }
            }
        }

        private static async Task CreateUsers(UserManager<IdentityUser> userManager)
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin@app.com",
                Email = "admin@app.com"
            };
            var normalUser = new IdentityUser
            {
                UserName = "user@app.com",
                Email = "user@app.com"
            };

            var adminResult = await userManager.CreateAsync(adminUser, "Password 123");

            if (adminResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, RoleNames.Admin.ToString());
            }

            var userResult = await userManager.CreateAsync(normalUser, "Password 123");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(normalUser, RoleNames.User.ToString());
            }
        }

        public static void Seed(IServiceProvider serviceProvider, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    CreateRoles(roleManager).Wait();
                    CreateUsers(userManager).Wait();
                }
            }
        }

    }
}
