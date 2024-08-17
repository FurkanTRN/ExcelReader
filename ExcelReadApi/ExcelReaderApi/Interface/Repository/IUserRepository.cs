using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IUserRepository
{
    Task CreateUser(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task<User> GetUserByEmailAsync(string email);
    Task DeleteUserAsync(int id);
    Task UpdateUserAsync(User user);
    Task<User> GetUserByCredentialsAsync(string username, string password);
    Task<User> GetUser(string email);
}