using ApiContracts.Users;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request);
    Task<UserDto> GetUserAsync(int id);
    Task<List<UserDto>> GetUsersAsync(string? userNameContains = null);
    Task DeleteUserAsync(int id);
}
