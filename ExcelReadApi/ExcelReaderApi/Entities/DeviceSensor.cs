using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Entities;

public class DeviceSensor
{
    public int Id { get; set; }
    public string? DeviceIdentity { get; set; }
    public Device Device { get; set; }
    public int? SensorId { get; set; }
    public Sensor? Sensor { get; set; }
}