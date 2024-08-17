using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Entities;

namespace ExcelReadApi.Interface;

public interface IDeviceRepository
{
    Task AddDeviceAsync(Device device);
    Task DeleteDeviceAsync(int deviceId);
    Task UpdateDeviceAsync(Device device);
    Task<Device> GetDeviceByIdAsync(int deviceId);
    Task<List<Device>> GetAllDevicesAsync();
    Task<List<Device>> GetDevicesNamesAsync();
    Task<List<Device>> GetDevicesByFileIdAsync(int fileId);
    Task<Device> GetDeviceByNameAsync(string deviceName, int userId);
    Task<Device> GetDeviceByFileIdAndNameAsync(string deviceName, int userId, int fileId);
    Task<List<Device>> GetDevicesByFileIdAndNameAsync(List<string> deviceNames, int userId, int fileId);
}