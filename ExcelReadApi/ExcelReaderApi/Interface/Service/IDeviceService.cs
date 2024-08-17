using ExcelReadApi.DTO.Device;
using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IDeviceService
{
    Task AddDeviceWithSensorsAsync(DevicesWithSensorDto? dto, int userId, int uploadedFileId);
    Task<List<DeviceDto>> GetAllDeviceAsync();
    Task DeleteDeviceAsync(int deviceId,int userId);
    Task<DeviceDto> GetDeviceWithSensors(int deviceId, int userId);
    Task<List<DeviceListDto>> GetDeviceListAsync(int userId, int fileId);
    Task<DevicesWithSensorDto> GetDeviceWithSensorsByNameAsync(string deviceName, int userId,int fileId);
    Task<List<MultipleSensorDto>> GetDevicesByFileIdAndNameAsync(List<string> deviceNames, int userId, int fileId);
}