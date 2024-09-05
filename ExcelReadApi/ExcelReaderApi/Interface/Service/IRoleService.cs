using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IRoleService
{
    Task AddRoleToUserAsync(int userId, int roleId);
    Task<Role> GetRoleByNameAsync(string roleName);
    Task EnsureRolesAsync(List<string> roles);

}