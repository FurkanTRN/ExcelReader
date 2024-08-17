namespace ExcelReadApi.DTO.Sensor;

public class MultipleSensorDto
{
    public string DeviceName { get; set; }
    public string Name { get; set; }
    public float Value { get; set; }
    public DateTime RecordDate { get; set; }
}