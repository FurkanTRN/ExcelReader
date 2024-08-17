using ExcelReadApi.DTO.Device;
using ExcelReadApi.DTO.Sensor;
using ExcelReadApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReadApi.Controllers;

[ApiController]
[Route("/api/device")]
[Authorize()]
public class DeviceController : Controller
{
    private readonly IDeviceService _deviceService;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;

    public DeviceController(IDeviceService deviceService, IConfiguration configuration, IUserService userService,
        IFileService fileService)
    {
        _deviceService = deviceService;
        _configuration = configuration;
        _userService = userService;
        _fileService = fileService;
    }

    [HttpGet("{deviceId}")]
    public async Task<IActionResult> GetDeviceWithSensors([FromRoute] int deviceId)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            var device = await _deviceService.GetDeviceWithSensors(deviceId, user.Id);
            return Ok(device);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet(Name = "devices")]
    public async Task<IActionResult> GetAllDevices()
    {
        try
        {
            var devices = await _deviceService.GetAllDeviceAsync();
            if (devices is null || !devices.Any())
            {
                return NotFound("No devices found");
            }

            return Ok(devices);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error:" + e.Message);
        }
    }

    [HttpGet("user/{fileId}")]
    public async Task<IActionResult> GetUserDevices([FromRoute] int fileId)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            var devices = await _deviceService.GetDeviceListAsync(user.Id, fileId);
            if (devices is null || !devices.Any())
            {
                return NotFound("No devices found");
            }

            return Ok(devices);
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error:" + e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDevice([FromRoute] int id)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            await _deviceService.DeleteDeviceAsync(id, user.Id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, "Internal server error: " + e.Message);
        }
    }

    [HttpGet("{deviceName}/file/{fileId}/sensors")]
    public async Task<ActionResult<List<SensorDto>>> GetSensorsByDeviceName(int fileId, string deviceName)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            var sensors = await _deviceService.GetDeviceWithSensorsByNameAsync(deviceName, user.Id, fileId);
            return Ok(sensors);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost("{fileId}/sensors")]
    public async Task<ActionResult<List<SensorDto>>> GetSensorsByDeviceNames(int fileId, [FromBody] List<string> deviceNames)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            var sensors = await _deviceService.GetDevicesByFileIdAndNameAsync(deviceNames, user.Id, fileId);
            return Ok(sensors);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}