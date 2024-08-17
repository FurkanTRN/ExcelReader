using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;


public class Device
{
  
    public int Id { get; set; }
    
    public string DeviceIdentity { get; set; }
    public string? Name { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public List<DeviceSensor> DeviceSensors { get; set; }
    public int UploadedFileId { get; set; } 
    public UploadedFile UploadedFile { get; set; } 
}
