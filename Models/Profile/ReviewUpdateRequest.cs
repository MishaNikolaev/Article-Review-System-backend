using System.ComponentModel.DataAnnotations;

public class ReviewUpdateRequest
{
    [Required]
    public List<int> Reviews { get; set; }
}