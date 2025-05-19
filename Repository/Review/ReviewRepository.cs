using Article_Review_System_backend.Data;
using Article_Review_System_backend.Repository.Review;
using Article_Review_System_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Article_Review_System_backend.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Review> CreateReviewAsync(Models.Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> ReviewExistsAsync(int id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _context.Reviews
                .AnyAsync(u => u.Id == id);
        }

        public async Task<Models.Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateReviewsAsync(Models.Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException(nameof(review));
            }

            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
    }
}