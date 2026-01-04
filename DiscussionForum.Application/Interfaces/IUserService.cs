using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.Interfaces;

/// <summary>
/// Service interface for user operations
/// </summary>
public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(string userId);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> UpdateUserProfileAsync(string userId, UpdateUserProfileDto dto);
    Task BanUserAsync(string userId);
    Task UnbanUserAsync(string userId);
    Task<int> GetTotalUsersCountAsync();
}
