using ExcelReadApi.Context;
using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class DeviceRepository : IDeviceRepository
{
    private readonly ExcelReaderApiDbContext _context;

    public DeviceRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }

    public async Task AddDeviceAsync(Device device)
    {
        _context.Devices.Add(device);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteDeviceAsync(int deviceId)
    {
        var device = await _context.Devices.FirstOrDefaultAsync(d => d.Id == deviceId);
        if (device is null) return;

        _context.Devices.Remove(device);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateDeviceAsync(Device device)
    {
        _context.Devices.Update(device);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Device>> GetDevicesNamesAsync()
    {
        return await _context.Devices.Include(d => d.DeviceSensors).ToListAsync();
    }
    
    public async Task<Device> GetDeviceByIdAsync(int deviceId)
    {
        return await _context.Devices
            .Include(r => r.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .FirstOrDefaultAsync(s => s.Id == deviceId);
    }

    public async Task<List<Device>> GetDevicesByFileIdAsync(int fileId)
    {
        return await _context.Devices
            .Where(d => d.UploadedFileId == fileId) 
            .Include(d => d.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .ToListAsync();
    }

    public async Task<Device> GetDeviceByNameAsync(string deviceName, int userId)
    {
        return await _context.Devices
            .Include(d => d.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .FirstOrDefaultAsync(d => d.Name == deviceName && d.UserId == userId);
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        return await _context.Devices
            .Include(r => r.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .ToListAsync();
    }
    public async Task<Device> GetDeviceByFileIdAndNameAsync(string deviceName, int userId, int fileId)
    {
        return await _context.Devices
            .Include(d => d.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .Where(d => d.Name == deviceName && d.UserId == userId && d.UploadedFileId == fileId)
            .FirstOrDefaultAsync();
    }
    public async Task<List<Device>> GetDevicesByFileIdAndNameAsync(List<string> deviceNames, int userId, int fileId)
    {
        return await _context.Devices
            .Include(d => d.DeviceSensors)
            .ThenInclude(ds => ds.Sensor)
            .Where(d => deviceNames.Contains(d.Name) && d.UserId == userId && d.UploadedFileId == fileId)
            .ToListAsync();
    }
    
}