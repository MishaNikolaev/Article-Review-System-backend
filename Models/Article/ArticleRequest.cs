using System.ComponentModel.DataAnnotations;

namespace Article_Review_System_backend.Models
{
    public class ArticleRequest
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? FeaturedImageUrl { get; set; }

        [Required]
        public string Category { get; set; }

        public string? Tags { get; set; }

        [Required]
        public int AuthorId { get; set; }
        
        public string? AttachmentUrl { get; set; }
    }
}