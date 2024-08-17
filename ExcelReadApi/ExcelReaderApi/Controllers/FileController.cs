using ExcelReadApi.DTO;
using ExcelReadApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReadApi.Controllers;

[ApiController]
[Route("/api/file")]
[Authorize]
public class FileController : Controller
{
    private readonly IExcelProcessingService _processingService;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    private readonly IFileService _fileService;

    public FileController(IExcelProcessingService processingService,IConfiguration configuration,IUserService userService,IFileService fileService)
    {
        _processingService = processingService;
        _configuration = configuration;
        _userService = userService;
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadExcelFile(IFormFile file)
    {
        var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
        if (userEmail == null)
        {
            return Unauthorized("User email not found in token.");
        }
        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return Unauthorized("User not verified");
        }
        
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        
        try
        {
            var fileDto = new FileDto
            {
                FileName = file.FileName,
                UserId = user.Id,
                UploadDate = DateTime.Now
            };
            await _fileService.AddUploadedFileAsync(fileDto, user.Id);
            var uploadedFile = (await _fileService.GetAllUploadedFilesAsync()).LastOrDefault(uf => uf.FileName == file.FileName);

            using (var stream = file.OpenReadStream())
            {
                await _processingService.ProcessExcelFileAsync(stream,user.Id,uploadedFile.Id);
                
            }
            
            return Ok("Excel file processed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error", error = ex.Message });        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDevices()
    {
        var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
        if (userEmail == null)
        {
            return Unauthorized("User email not found in token.");
        }
        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return Unauthorized("User not verified");
        }

        try
        {
            var files = await _fileService.GetUserFilesAsync(user.Id);
            return Ok(files);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Internal server error", error = e.Message });
        }

    }

    [HttpDelete("{fileId}")]
    public async Task<IActionResult> DeleteFile([FromRoute] int fileId)
    {
        var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
        if (userEmail == null)
        {
            return Unauthorized("User email not found in token.");
        }
        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user == null)
        {
            return Unauthorized("User not verified");
        }

        try
        {
            await _fileService.DeleteFileAsync(fileId, user.Id);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { message = "Internal server error", error = e.Message });
        }
    }
}