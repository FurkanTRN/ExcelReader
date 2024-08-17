using ExcelReadApi.DTO;
using ExcelReadApi.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReadApi.Controllers;
[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            await _authService.RegisterAsync(dto);
            return Ok(new { Message = "User registered successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}