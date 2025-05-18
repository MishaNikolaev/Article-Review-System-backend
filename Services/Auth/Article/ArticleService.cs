using Article_Review_System_backend.Models;
using Article_Review_System_backend.Repository;
using Article_Review_System_backend.Repository.User;

namespace Article_Review_System_backend.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IUserRepository _userRepository;

        public ArticleService(IArticleRepository articleRepository, IUserRepository userRepository)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
        }

        public async Task<Article> SubmitArticleAsync(ArticleRequest request)
        {
            var author = await _userRepository.GetUserByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new ApplicationException("Author not found");
            }

            var article = new Article
            {
                Title = request.Title,
                Content = request.Content,
                FeaturedImageUrl = request.FeaturedImageUrl,
                Category = request.Category,
                Tags = request.Tags,
                AuthorId = request.AuthorId
            };

            return await _articleRepository.CreateArticleAsync(article);
        }

        public async Task<IEnumerable<ArticleResponse>> GetUserArticlesAsync(int authorId)
        {
            var articles = await _articleRepository.GetArticlesByAuthorAsync(authorId);
            
            return articles.Select(a => new ArticleResponse
            {
                Id = a.Id,
                Title = a.Title,
                SubmittedDate = a.SubmittedDate.ToString("MMMM d, yyyy"),
                Category = a.Category,
                Status = a.Status
            });
        }
    }
}