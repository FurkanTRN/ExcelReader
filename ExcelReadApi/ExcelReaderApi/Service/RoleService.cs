using ExcelReadApi.Entities;
using ExcelReadApi.Interface;

namespace ExcelReadApi.Service;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task AddRoleToUserAsync(int userId, int roleId)
    {
        var userRole = new UserRole()
        {
            UserId = userId,
            RoleId = roleId
        };
        await _roleRepository.AddRoleToUser(userRole);
    }

    public async Task EnsureRolesAsync(List<string> roles)
    {
        await _roleRepository.EnsureRolesAsync(roles);
    }

    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleRepository.GetRoleByNameAsync(roleName);
        if (role is null)
        {
            throw new Exception("Role not found");
        }

        return role;
    }

}