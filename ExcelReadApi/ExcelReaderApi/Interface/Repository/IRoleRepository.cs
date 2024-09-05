using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IRoleRepository
{
    Task AddRoleToUser(UserRole userRole);
    Task<Role> GetRoleByNameAsync(string roleName);
    Task EnsureRolesAsync(List<string> roles);


}