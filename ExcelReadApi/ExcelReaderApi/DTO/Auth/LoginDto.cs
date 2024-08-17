using System.ComponentModel.DataAnnotations;

namespace ExcelReadApi.DTO;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "Please enter valid email address")]
    public string Email { get; set; }
    [Required]
    [StringLength(25),MinLength(3,ErrorMessage = "Please enter at least 8 characters")]
    public string Password { get; set; }
}