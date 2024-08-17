using ExcelReadApi.DTO;
using ExcelReadApi.Entities;
using ExcelReadApi.Interface;

namespace ExcelReadApi.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleService _roleService;

    public UserService(IUserRepository userRepository,IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleService = roleService;
    }
    public async Task<CreateUserResponseDto> CreateUser(CreateUserDto dto)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new Exception("Email is already taken");
        }
        var user = new User()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Password = HashPassword(dto.Password),
            CreatedDate = DateTime.Now
        };
        await _userRepository.CreateUser(user);
        var userRole = await _roleService.GetRoleByNameAsync("User");
        await _roleService.AddRoleToUserAsync(user.Id, userRole.Id);

        return new CreateUserResponseDto()
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedDate = user.CreatedDate
        };
    }

    public async Task<List<UsersDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userDtos = users.Select(user => new UsersDto()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.Password,
            CreatedDate = user.CreatedDate,
            Roles = user.UserRoles!.Select(ur => ur.Role!.Name).ToList(),
            Devices = user.Devices!.Select(d=>new DevicesDto()
            {
                Id = d.Id,
                Name = d.Name,
                UserId = d.UserId
            }).ToList()
        }).ToList();
        return userDtos;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user is null)
        {
            throw new ArgumentException("User not found");
        }

        return user;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        return user;
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }

    public async Task UpdateUserAsync(int id,UpdateUserRequestDto dto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser is null)
        {
            throw new Exception("User not found");
        }

        existingUser.FirstName = dto.FirstName;
        existingUser.LastName = dto.LastName;
        existingUser.Password = HashPassword(dto.Password);
        await _userRepository.UpdateUserAsync(existingUser);
    }

    public async Task AssignRoleToUser(AssignRoleDto assignRoleDto)
    {
        var role = await _roleService.GetRoleByNameAsync(assignRoleDto.RoleName);
        await _roleService.AddRoleToUserAsync(assignRoleDto.UserId, role.Id);
    }

    public async Task<User> GetUserByCredentialsAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user is null || !VerifyPassword(password,user.Password!))
        {
            throw new Exception("User credentials are wrong!");
        }

        return user;
    }

    public async Task<User> GetUser(string email)
    {
        return await _userRepository.GetUser(email);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    
    
}