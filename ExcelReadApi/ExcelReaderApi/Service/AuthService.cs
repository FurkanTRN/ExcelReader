using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExcelReadApi.DTO;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;
using Microsoft.IdentityModel.Tokens;

namespace ExcelReadApi.Service;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthService(IConfiguration configuration,IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }
    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userService.GetUserByCredentialsAsync(dto.Email, dto.Password);
        if (user is null) return null;
        
        var claims = GenerateClaims(user);
        var token = GenerateToken(claims);

        return await token;

    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var existUser = await _userService.GetUserByEmailAsync(dto.Email);
        if (existUser != null)
        {
            throw new InvalidOperationException("User already exist");

        }

        var user = new CreateUserDto()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = dto.Password
        };
        await _userService.CreateUser(user);

    }
    private static IEnumerable<Claim> GenerateClaims(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (user.UserRoles != null && user.UserRoles.Any())
        {
            claims.AddRange(user.UserRoles
                .Where(ur => ur.Role != null)
                .Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));
        }

        return claims;
    }

    private async Task<string> GenerateToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            notBefore:DateTime.Now,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}