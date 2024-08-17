using System.ComponentModel.DataAnnotations;

namespace ExcelReadApi.DTO;

public class RegisterDto
{
    [Required] [MinLength(3)] public string FirstName { get; set; }
    [Required] [MinLength(3)] public string LastName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(25), MinLength(6)]
    public string Password { get; set; }
}