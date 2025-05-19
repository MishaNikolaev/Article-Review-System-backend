using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models
{
    public class ReviewRequest
    {
        [Required]
        public int ReviewerId { get; set; }
        public int ArticleId { get; set; }
        public string Status { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        public int Rating { get; set; }

        public string? TechnicalMerit { get; set; }
        public string? Originality { get; set; }
        public string? PresentationQuality { get; set; }
        public string? CommentsToAuthor { get; set; }
        public string? CommentsToEditor { get; set; }
        public string? AttachmentUrl { get; set; }
    }
}