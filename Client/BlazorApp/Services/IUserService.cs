using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService
{
    Task<UserDto> AddUserAsync(CreateUserDto request);
    Task<IEnumerable<UserDto>> GetUsersAsync(string? username = null);
    Task<UserDto> GetUserAsync(int id);
    Task UpdateUserAsync(int id, UpdateUserDto request);
    Task DeleteUserAsync(int id);
}