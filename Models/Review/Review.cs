using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int ReviewerId { get; set; }
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public Article? ArticleBase { get; set; }

        [Required]
        public User? Author { get; set; }

        [Required]
        public string Status { get; set; } = "Accepted";

        [Required]
        public DateTime DueDate { get; set; }

        public int Rating { get; set; } = 0;

        public string? TechnicalMerit { get; set; } = "";
        public string? Originality { get; set; } = "";
        public string? PresentationQuality { get; set; } = "";
        public string? CommentsToAuthor { get; set; } = "";
        public string? CommentsToEditor { get; set; } = "";
        public string? AttachmentUrl { get; set; } = "";
    }
}