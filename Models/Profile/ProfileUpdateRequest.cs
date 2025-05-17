using System.ComponentModel.DataAnnotations;

public class ProfileUpdateRequest
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }
    
    public string? Specialization { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string? Twitter { get; set; }
    public string? LinkedIn { get; set; }
}