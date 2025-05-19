using Article_Review_System_backend.Models;

namespace Article_Review_System_backend.Services
{
    public interface IReviewService
    {
        Task<Review> SubmitReviewAsync(ReviewRequest request);
    }
}