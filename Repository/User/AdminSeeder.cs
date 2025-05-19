using Article_Review_System_backend.Repository.User;
using Article_Review_System_backend.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace Article_Review_System_backend.Repository.User
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(IUserRepository userRepository, IConfiguration configuration)
        {
            var adminConfig = configuration.GetSection("AdminCredentials");
            var email = adminConfig["Email"];
            var password = adminConfig["Password"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new InvalidOperationException("Admin credentials are not set properly.");
            }

            var existingAdmin = await userRepository.GetUserByEmailAsync(email);
            if (existingAdmin != null) return;
            Console.WriteLine("Admin already exists: " + email);


            using var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var adminUser = new Article_Review_System_backend.Models.User
            {
                FirstName = "Admin",
                LastName = "User",
                Gender = "Other",
                Email = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Admin"
            };
            Console.WriteLine("Admin created with email: " + email);


            await userRepository.CreateUserAsync(adminUser);
        }
    }
}