using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Attempting to apply migration...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if (!context.Users.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                CreatePasswordHash("tes_password", out byte[] passwordHash1, out byte[] passwordSalt1);
                CreatePasswordHash("tes_password", out byte[] passwordHash2, out byte[] passwordSalt2);
                CreatePasswordHash("tes_password", out byte[] passwordHash3, out byte[] passwordSalt3);
                CreatePasswordHash("tes_password", out byte[] passwordHash4, out byte[] passwordSalt4);
                context.Users.AddRange(
                    new User { Username = "ToM_B", PasswordHash = passwordHash1, PasswordSalt = passwordSalt1, Role = "Admin" },
                    new User { Username = "Bob1023", PasswordHash = passwordHash2, PasswordSalt = passwordSalt2, Role = "Member" },
                    new User { Username = "Business_A", PasswordHash = passwordHash3, PasswordSalt = passwordSalt3, Role = "Business" },
                    new User { Username = "Mary000", PasswordHash = passwordHash4, PasswordSalt = passwordSalt4, Role = "Member" }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have user data");
            }

        }
        private static void CreatePasswordHash(string v, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(v));
            }
        }
    }
}