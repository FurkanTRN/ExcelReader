using ExcelReadApi.DTO.Device;
using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;


namespace ExcelReadApi.Service;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IFileService _fileService;
    private readonly ISensorRepository _sensorRepository;

    public DeviceService(IDeviceRepository deviceRepository, IFileService fileService,ISensorRepository sensorRepository)
    {
        _deviceRepository = deviceRepository;
        _fileService = fileService;
        _sensorRepository = sensorRepository;
    }

    public async Task AddDeviceWithSensorsAsync(DevicesWithSensorDto? dto, int userId, int fileId)
    {
        var uploadedFile = await _fileService.GetFileByIdAsync(fileId, userId);
        if (uploadedFile is null)
        {
            throw new ArgumentException("File not found");
        }

        var device = new Device
        {
            Name = dto.DeviceName,
            DeviceIdentity = dto.DeviceId,
            UserId = userId,
            UploadedFileId = fileId,
            DeviceSensors = dto.Sensors.Select(sensor => new DeviceSensor
            {
                Sensor = new Sensor
                {
                    Name = sensor.Name,
                    Value = sensor.Value,
                    RecordDate = sensor.RecordDate
                },
            }).ToList(),
        };
        await _deviceRepository.AddDeviceAsync(device);
    }

    public async Task<List<DeviceDto>> GetAllDeviceAsync()
    {
        var devices = await _deviceRepository.GetAllDevicesAsync();
        var deviceDto = devices.Select(d => new DeviceDto()
        {
            Id = d.Id,
            DeviceId = d.DeviceIdentity,
            DeviceName = d.Name,
            UserId = d.UserId,
            FileId = d.UploadedFileId,
            Sensors = d.DeviceSensors.Select(ds => new SensorDto()
            {
                Name = ds.Sensor.Name,
                Value = ds.Sensor.Value,
                RecordDate = ds.Sensor.RecordDate
            }).ToList()
        }).ToList();
        return deviceDto;
    }


    public async Task<DeviceDto> GetDeviceWithSensors(int deviceId, int userId)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceId);
        if (device is null)
        {
            throw new Exception("Device not found");
        }

        var deviceDto = new DeviceDto()
        {
            Id = device.Id,
            DeviceId = device.DeviceIdentity,
            DeviceName = device.Name,
            UserId = userId,
            Sensors = device.DeviceSensors
                .Select(ds => new SensorDto()
                {
                    Name = ds.Sensor.Name,
                    Value = ds.Sensor.Value,
                    RecordDate = ds.Sensor.RecordDate
                }).ToList()
        };
        return deviceDto;
    }

    public async Task DeleteDeviceAsync(int deviceId, int userId)
    {
        var device = await _deviceRepository.GetDeviceByIdAsync(deviceId);
        if (device is null)
        {
            throw new ArgumentException("Device not found");
        }

        device.UserId = userId;
        await _deviceRepository.DeleteDeviceAsync(deviceId);
    }

    public async Task<List<DeviceListDto>> GetDeviceListAsync(int userId, int fileId)
    {
        var devices = await _deviceRepository.GetDevicesByFileIdAsync(fileId);
        var deviceList = devices
            .Where(s => s.UserId == userId)
            .GroupBy(d => d.Name)
            .Select(g => g.First())
            .Select(d => new DeviceListDto()
            {
                Name = d.Name,
                Id = d.Id
            }).ToList();
        return deviceList;
    }

    public async Task<DevicesWithSensorDto> GetDeviceWithSensorsByNameAsync(string deviceName, int userId, int fileId)
    {
        var device = await _deviceRepository.GetDeviceByNameAsync(deviceName, userId);
        if (device == null)
        {
            throw new ArgumentException("Device not found");
        }

        var deviceWithSensorsDto = new DevicesWithSensorDto
        {
            DeviceId = device.DeviceIdentity,
            DeviceName = device.Name,
            Sensors = device.DeviceSensors.Select(ds => new SensorDto
            {
                Name = ds.Sensor.Name,
                Value = ds.Sensor.Value,
                RecordDate = ds.Sensor.RecordDate
            }).ToList()
        };

        return deviceWithSensorsDto;
    }

    public async Task<DevicesWithSensorDto> GetDeviceWithSensorsByNameAsync(string deviceName, int userId)
    {
        var device = await _deviceRepository.GetDeviceByNameAsync(deviceName, userId);
        if (device == null)
        {
            throw new ArgumentException("Device not found");
        }

        var deviceWithSensorsDto = new DevicesWithSensorDto
        {
            DeviceId = device.DeviceIdentity,
            DeviceName = device.Name,
            Sensors = device.DeviceSensors.Select(ds => new SensorDto
            {
                Name = ds.Sensor.Name,
                Value = ds.Sensor.Value,
                RecordDate = ds.Sensor.RecordDate
            }).ToList()
        };

        return deviceWithSensorsDto;
    }

    public async Task<DevicesWithSensorDto> GetDeviceWithSensorsByFileIdAndNameAsync(string deviceName, int userId,
        int fileId)
    {
        var device = await _deviceRepository.GetDeviceByFileIdAndNameAsync(deviceName, userId, fileId);
        if (device == null)
        {
            throw new ArgumentException("Device not found");
        }

        var deviceWithSensorsDto = new DevicesWithSensorDto
        {
            DeviceId = device.DeviceIdentity,
            DeviceName = device.Name,
            Sensors = device.DeviceSensors.Select(ds => new SensorDto
            {
                Name = ds.Sensor.Name,
                Value = ds.Sensor.Value,
                RecordDate = ds.Sensor.RecordDate
            }).ToList()
        };

        return deviceWithSensorsDto;
    }

    public async Task<List<SensorDto>> GetSensorsByDeviceNameAsync(List<string> deviceName, int userId, int fileId)
    {
        var devices = await _deviceRepository.GetDevicesByFileIdAndNameAsync(deviceName, userId, fileId);

        if (devices == null || !devices.Any())
        {
            throw new ArgumentException("No devices found");
        }

        var sensors = devices.SelectMany(d => d.DeviceSensors)
            .Select(ds => new SensorDto
            {
                Name = ds.Sensor.Name,
                Value = ds.Sensor.Value,
                RecordDate = ds.Sensor.RecordDate
            }).ToList();

        return sensors;
    }

    public async Task<List<MultipleSensorDto>> GetDevicesByFileIdAndNameAsync(List<string> deviceNames, int userId,
        int fileId)
    {
        var devices = await _deviceRepository.GetDevicesByFileIdAndNameAsync(deviceNames, userId, fileId);
        var sensors = devices
            .SelectMany(d => d.DeviceSensors
                .Select(ds => new MultipleSensorDto()
                {
                    DeviceName = d.Name,
                    Name = ds.Sensor.Name,
                    Value = ds.Sensor.Value,
                    RecordDate = ds.Sensor.RecordDate
                })).OrderBy(s=>s.DeviceName)
            .ToList();
        return sensors;
    }
}