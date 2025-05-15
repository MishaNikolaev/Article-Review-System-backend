// Repository/User/IUserRepository.cs
using Article_Review_System_backend.Models;

namespace Article_Review_System_backend.Repository.User
{
    public interface IUserRepository
    {
        Task<Models.User> CreateUserAsync(Models.User user);
        Task<Models.User?> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
    }
}