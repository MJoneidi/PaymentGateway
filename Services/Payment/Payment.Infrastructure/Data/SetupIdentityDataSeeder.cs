using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payment.Infrastructure.Data
{
    public class StartupDbInitializer
    {
        private const string merchantEmail = "merchant@mail.com";
        private const string merchantPassword = "StrongPasswordMerchant123!";

        private static readonly List<IdentityRole> Roles = new List<IdentityRole>()
        {
            new IdentityRole {Name = "Merchant", NormalizedName = "MERCHANT", ConcurrencyStamp = Guid.NewGuid().ToString()}
        };

        public static void SeedData(PaymentDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            dbContext.Database.EnsureCreated();
            AddRoles(dbContext);
            AddUser(dbContext, userManager);
            AddUserRoles(dbContext, userManager);
        }

        private static void AddRoles(PaymentDbContext dbContext)
        {
            if (!dbContext.Roles.Any())
            {
                foreach (var role in Roles)
                {
                    dbContext.Roles.Add(role);
                    dbContext.SaveChanges();
                }
            }
        }

        private static void AddUser(PaymentDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            if (!dbContext.Users.Any())
            {
                var merchant = new IdentityUser
                {
                    Id = new Guid().ToString(),
                    UserName = merchantEmail,
                    NormalizedUserName = "MERCHANT",
                    Email = merchantEmail,
                    NormalizedEmail = merchantEmail.ToUpper(),
                    PhoneNumber = "XXXXXXXXXXXXX",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = new Guid().ToString("D")
                };

                merchant.PasswordHash = PassGenerate(merchant, merchantPassword);

                userManager.CreateAsync(merchant).Wait();
                dbContext.SaveChanges();
            }
        }

        public static string PassGenerate(IdentityUser user, string password)
        {
            var passHash = new PasswordHasher<IdentityUser>();
            return passHash.HashPassword(user, password);
        }

        private static void AddUserRoles(PaymentDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            if (!dbContext.UserRoles.Any())
            {
                var merchantRole = new IdentityUserRole<string>
                {
                    UserId = dbContext.Users.Single(r => r.Email == merchantEmail).Id,
                    RoleId = dbContext.Roles.Single(r => r.Name == "Merchant").Id
                };

                dbContext.UserRoles.Add(merchantRole);
                dbContext.SaveChanges();
            }
        }
    }
}