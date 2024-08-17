using System.ComponentModel.DataAnnotations;

namespace ExcelReadApi.DTO;

public class UpdateUserRequestDto
{
    [Required] [MinLength(3)] public string FirstName { get; set; }
    [Required] [MinLength(3)] public string LastName { get; set; }

    [Required]
    [StringLength(25), MinLength(6)]
    public string Password { get; set; }
    
}