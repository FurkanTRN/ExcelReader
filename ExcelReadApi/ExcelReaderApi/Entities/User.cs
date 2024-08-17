using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;

public  class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public DateTime CreatedDate { get; set; }
    
    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();
    public virtual ICollection<UploadedFile> UploadedFiles { get; set; } = new List<UploadedFile>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public virtual ICollection<PrintHistory> PrintHistories { get; set; } = new List<PrintHistory>(); // Added collection
}
