using Article_Review_System_backend.Models;
using Article_Review_System_backend.Repository;
using Article_Review_System_backend.Repository.Review;

namespace Article_Review_System_backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IArticleRepository _articleRepository;
        public ReviewService(IReviewRepository reviewRepository, IArticleRepository articleRepository)
        {
            _reviewRepository = reviewRepository;
            _articleRepository = articleRepository;
        }

        public async Task<Review> SubmitReviewAsync(ReviewRequest request)
        {
            var article = await _articleRepository.GetArticleByIdAsync(request.ArticleId);
            if (article == null)
            {
                throw new ApplicationException("Article not found");
            }

            var review = new Review
            {
                ReviewerId = request.ReviewerId,
                ArticleId = request.ArticleId,
                ArticleBase = article,
                Status = request.Status,
                TechnicalMerit = request.TechnicalMerit,
                Originality = request.Originality,
                PresentationQuality = request.PresentationQuality,
                CommentsToAuthor = request.CommentsToAuthor,
                CommentsToEditor = request.CommentsToEditor,
                AttachmentUrl = request.AttachmentUrl,
            };

            return await _reviewRepository.CreateReviewAsync(review);
        }
    }
}