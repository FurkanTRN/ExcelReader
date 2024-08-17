using ExcelReadApi.DTO;

namespace ExcelReadApi.Interface;

public interface IAuthService
{
    Task<string> LoginAsync(LoginDto dto);
    Task RegisterAsync(RegisterDto dto);
}