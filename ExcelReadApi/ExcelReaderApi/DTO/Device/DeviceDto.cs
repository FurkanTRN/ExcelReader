using ExcelReadApi.DTO.Sensor;

namespace ExcelReadApi.DTO.Device;

public class DeviceDto
{
    public int Id { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public int UserId { get; set; }
    public int FileId { get; set; }
    public List<SensorDto> Sensors { get; set; }
}