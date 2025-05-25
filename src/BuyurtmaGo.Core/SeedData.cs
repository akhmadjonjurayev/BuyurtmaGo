using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace BuyurtmaGo.Core
{
    public record UserModel(string FirstName, 
        string LastName,
        string Username,
        string Email,
        string PhoneNumber, 
        Roles Role, 
        string Password);

    public static class SeedData
    {
        public static void Seed(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<BuyurtmaGoDb>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            AddRoles(dbContext, roleManager);
            AddUsers(dbContext, userManager);
        }

        private static void AddRoles(BuyurtmaGoDb _dbContext, RoleManager<Role> roleManager)
        {
            if (!_dbContext.Roles.Any())
            {
                roleManager.CreateAsync(new Role
                {
                    Name = nameof(Roles.Administrator)
                }).GetAwaiter().GetResult();

                roleManager.CreateAsync(new Role
                {
                    Name = nameof(Roles.Manager)
                }).GetAwaiter().GetResult();

                roleManager.CreateAsync(new Role
                {
                    Name = nameof(Roles.Agent)
                }).GetAwaiter().GetResult();
            }
        }

        private static void AddUsers(BuyurtmaGoDb _dbContext, UserManager<User> userManager)
        {
            TryAddUser(userManager, new UserModel(FirstName: "Axmadjon", LastName: "Jo'rayev", Username: "kalinus", Email: "kalinus2775@gmail.com", PhoneNumber: "+998946644275", Role: Roles.Administrator, Password: "1"));
        }

        private static void TryAddUser(UserManager<User> userManager, UserModel userView)
        {
            var user = userManager.FindByNameAsync(userView.Username).GetAwaiter().GetResult();
            if (user is null)
            {
                user = new User
                {
                    FirstName = userView.FirstName,
                    LastName = userView.LastName,
                    UserName = userView.Username,
                    Email = userView.Email,
                    PhoneNumber = userView.PhoneNumber
                };
                var result = userManager.CreateAsync(user, userView.Password).GetAwaiter().GetResult();
                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, userView.Role.ToString()).GetAwaiter().GetResult();
            }
        }
    }
}
