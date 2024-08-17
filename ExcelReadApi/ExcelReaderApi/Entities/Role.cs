using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public IEnumerable<UserRole>? UserRoles { get; set; }
}
