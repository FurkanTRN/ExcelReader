using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;

public class UploadedFile
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? FileName { get; set; }
    public DateTime UploadDate { get; set; }
    public virtual ICollection<DeviceSensor> DeviceSensors { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PrintHistory> PrintHistories { get; set; } = new List<PrintHistory>(); // Added collection
}