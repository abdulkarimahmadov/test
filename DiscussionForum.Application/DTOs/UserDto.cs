using DiscussionForum.Domain.Enums;

namespace DiscussionForum.Application.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Avatar { get; set; }
    public int ReputationPoints { get; set; }
    public UserRole Role { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool IsBanned { get; set; }
    public int TopicCount { get; set; }
    public int PostCount { get; set; }
}

public class UpdateUserProfileDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Avatar { get; set; }
}
