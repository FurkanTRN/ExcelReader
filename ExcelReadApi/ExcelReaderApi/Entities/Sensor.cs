using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;

public partial class Sensor
{
    
    public int Id{ get; set; }
    public string? Name { get; set; }
    public float Value { get; set; }
    public DateTime RecordDate { get; set; }

    public IEnumerable<DeviceSensor>? DeviceSensors { get; set; }
}
