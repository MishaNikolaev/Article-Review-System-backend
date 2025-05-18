using Article_Review_System_backend.Models;

namespace Article_Review_System_backend.Repository
{
    public interface IArticleRepository
    {
        Task<Article> CreateArticleAsync(Article article);
        Task<IEnumerable<Article>> GetArticlesByAuthorAsync(int authorId);
        Task<Article?> GetArticleByIdAsync(int id);
    }
}