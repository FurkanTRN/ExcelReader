using ExcelReadApi.Context;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.EntityFrameworkCore;

namespace ExcelReadApi.Repository;

public class SensorRepository : ISensorRepository
{
    private readonly ExcelReaderApiDbContext _context;

    public SensorRepository(ExcelReaderApiDbContext context)
    {
        _context = context;
    }

    public async Task AddSensorAsync(Sensor sensor)
    {
        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSensorAsync(Sensor sensor)
    {
        _context.Sensors.Update(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSensorAsync(int sensorId)
    {
        var sensor = await _context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
        if (sensor is null) return;
        _context.Sensors.Remove(sensor);
        await _context.SaveChangesAsync();
    }

    public async Task<Sensor> GetSensorByIdAsync(int sensorId)
    {
        return await _context.Sensors.FirstOrDefaultAsync(s => s.Id == sensorId);
    }

    
}
