using Article_Review_System_backend.Models;
using Article_Review_System_backend.Models.Auth;
using Article_Review_System_backend.Repository;
using System.Security.Cryptography;
using System.Text;
using Article_Review_System_backend.Repository.User;

namespace Article_Review_System_backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.UserExistsAsync(request.Email))
            {
                throw new ApplicationException("Email already exists");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var avatarUrl = $"https://api.dicebear.com/7.x/initials/svg?seed={request.FirstName}+{request.LastName}";

            var user = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                AvatarUrl = avatarUrl,
                Role = request.Role ?? "Author"
            };

            var createdUser = await _userRepository.CreateUserAsync(user);

            return new AuthResponse
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Role = createdUser.Role,
                AvatarUrl = createdUser.AvatarUrl,
                Gender = createdUser.Gender
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ApplicationException("User not found");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new ApplicationException("Password is incorrect");
            }

            return new AuthResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                Specialization = user.Specialization,
                Location = user.Location,
                Bio = user.Bio,
                Twitter = user.Twitter,
                LinkedIn = user.LinkedIn,
                AvatarUrl = user.AvatarUrl
            };
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}