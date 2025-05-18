using Article_Review_System_backend.Models;

namespace Article_Review_System_backend.Services
{
    public interface IArticleService
    {
        Task<Article> SubmitArticleAsync(ArticleRequest request);
        Task<IEnumerable<ArticleResponse>> GetUserArticlesAsync(int authorId);
    }
}