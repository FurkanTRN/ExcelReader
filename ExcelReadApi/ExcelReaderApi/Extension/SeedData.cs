using ExcelReadApi.Interface;

namespace ExcelReadApi.Extension;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleService = serviceProvider.GetRequiredService<IRoleService>();
        var roles = new List<string> { "Admin", "User"};
        await roleService.EnsureRolesAsync(roles);
    }
}