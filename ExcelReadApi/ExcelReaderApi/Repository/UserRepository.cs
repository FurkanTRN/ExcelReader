using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class UserRepository : IUserRepository
{
    private ExcelReaderApiDbContext _context;

    public UserRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }

    public async Task CreateUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u=>u.Devices)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.Include(e=>e.Devices).Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.
            Include(f=>f.UploadedFiles)
            .Include(f=>f.PrintHistories)
            .Include(e=>e.Devices).Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(s => s.Email == email);
    }

    public async Task<User> GetUser(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(s => s.Email == email);
    }
    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (user is null) return;
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetUserByCredentialsAsync(string email, string password)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
    }

    public async Task<int> UserCountAsync()
    {
        return await _context.Users.CountAsync();
    }
}