using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface ISensorRepository
{
    Task AddSensorAsync(Sensor sensor);
    Task UpdateSensorAsync(Sensor sensor);
    Task DeleteSensorAsync(int sensorId);
    Task<Sensor> GetSensorByIdAsync(int sensorId);
    
}