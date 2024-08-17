using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Entities;

namespace ExcelReadApi.DTO.Device;

public class AllDeviceDto
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public int UserId { get; set; }

    public List<Entities.Sensor> DeviceSensors { get; set; }
}