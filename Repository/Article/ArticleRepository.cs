using Article_Review_System_backend.Data;
using Article_Review_System_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Article_Review_System_backend.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly AppDbContext _context;

        public ArticleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Article> CreateArticleAsync(Article article)
        {
            if (article == null)
            {
                throw new ArgumentNullException(nameof(article));
            }

            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<IEnumerable<Article>> GetArticlesByAuthorAsync(int authorId)
        {
            return await _context.Articles
                .Where(a => a.AuthorId == authorId)
                .OrderByDescending(a => a.SubmittedDate)
                .ToListAsync();
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _context.Articles
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}