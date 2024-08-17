﻿using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class RoleRepository : IRoleRepository
{
    private readonly ExcelReaderApiDbContext _context;
    public RoleRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }
    public async Task AddRoleToUser(UserRole userRole)
    {
        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

    }

   
    public async Task<Role> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles.FirstOrDefaultAsync(s => s.Name == roleName);
    }

  
}