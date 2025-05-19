using Article_Review_System_backend.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Article_Review_System_backend.Repository.Review
{
    public interface IReviewRepository
    {
        Task<Models.Review> CreateReviewAsync(Models.Review review);
        Task<Models.Review?> GetReviewByIdAsync(int id);
        Task<bool> ReviewExistsAsync(int id);
        Task UpdateReviewsAsync(Models.Review review);
    }
}