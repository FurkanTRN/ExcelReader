using ExcelReadApi.DTO.Sensor;

namespace ExcelReadApi.DTO.Device;

public class DevicesWithSensorDto
{
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public List<SensorDto> Sensors { get; set; }
}