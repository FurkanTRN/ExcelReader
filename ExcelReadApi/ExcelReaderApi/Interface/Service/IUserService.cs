using ExcelReadApi.DTO;
using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IUserService
{
    Task<CreateUserResponseDto> CreateUser(CreateUserDto dto);
    Task<List<UsersDto>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task DeleteUserAsync(int id);
    Task UpdateUserAsync(int id,UpdateUserRequestDto dto);
    Task AssignRoleToUser(AssignRoleDto assignRoleDto);
    Task<User> GetUserByCredentialsAsync(string email, string password);
    Task<User> GetUser(string email);
 
}