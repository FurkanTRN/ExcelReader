using System;
using System.Collections.Generic;
using ExcelReadApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Context;

public partial class ExcelReaderApiDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ExcelReaderApiDbContext(DbContextOptions<ExcelReaderApiDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("SqlServer"));
    }

    public virtual DbSet<DeviceSensor> DeviceSensors { get; set; }
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Sensor> Sensors { get; set; }
    public virtual DbSet<UploadedFile> UploadedFiles { get; set; }
    public virtual DbSet<UserRole> UserRoles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<PrintHistory> PrintHistories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<Device>()
            .HasKey(d => d.Id);

        modelBuilder.Entity<DeviceSensor>()
            .HasKey(ds => ds.Id);

        modelBuilder.Entity<Sensor>()
            .HasKey(s => s.Id);
        
        modelBuilder.Entity<PrintHistory>()
            .HasKey(ph => ph.Id);

        modelBuilder.Entity<PrintHistory>().HasOne(ph => ph.UploadedFile)
            .WithMany(uf => uf.PrintHistories)
            .HasForeignKey(ph => ph.FileId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<PrintHistory>()
            .HasOne(ph => ph.User)
            .WithMany(u => u.PrintHistories) 
            .HasForeignKey(ph => ph.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<DeviceSensor>()
            .HasOne(ds => ds.Sensor)
            .WithMany(s => s.DeviceSensors)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(ds => ds.SensorId);

        modelBuilder.Entity<Device>()
            .HasOne(d => d.User)
            .WithMany(u => u.Devices)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey(d => d.UserId);

        modelBuilder.Entity<UploadedFile>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UploadedFiles)
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(uf => uf.UserId);
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}