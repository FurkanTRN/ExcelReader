using System.Security.Claims;
using ExcelReadApi.DTO;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReadApi.Controllers;

[Authorize]
[ApiController]
[Route("api/history")]
public class PrintHistoryController : Controller
{
    private readonly IPrintHistoryService _printHistoryService;
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public PrintHistoryController(IPrintHistoryService printHistoryService, IUserService userService,
        IConfiguration configuration)
    {
        _printHistoryService = printHistoryService;
        _userService = userService;
        _configuration = configuration;
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<PrintHistoryResponse>>> GetPrintHistories()
    {
        var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
        var user = await _userService.GetUserByEmailAsync(userEmail);
        if (user is null)
        {
            return Unauthorized("User not verified");
        }

        var printHistories = await _printHistoryService.GetPrintHistoriesAsync(user.Id);

        if (printHistories == null || !printHistories.Any())
        {
            return NotFound();
        }

        return Ok(printHistories);
    }

    [HttpPost]
    public async Task<ActionResult> AddPrintHistory([FromBody] CreatePrintHistoryDto dto)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            await _printHistoryService.AddPrintHistoryAsync(dto, user.Id);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PrintHistoryResponse>> GetPrintHistoryById(int id)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            var printHistory=await _printHistoryService.GetPrintHistoryByIdAsync(id, user.Id);

            return Ok(printHistory);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }     
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrintHistoryById(int id)
    {
        try
        {
            var userEmail = User.FindFirst(_configuration["Jwt:EmailTemplate"]).Value;
            var user = await _userService.GetUserByEmailAsync(userEmail);
            if (user is null)
            {
                return Unauthorized("User not verified");
            }

            await _printHistoryService.DeletePrintHistoryAsync(id,user.Id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}