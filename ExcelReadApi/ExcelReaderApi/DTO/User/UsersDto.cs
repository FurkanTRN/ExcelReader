namespace ExcelReadApi.DTO;

public class UsersDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<string?> Roles { get; set; }
    public List<DevicesDto> Devices { get; set; }
}