using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DiscussionForum.Application.Services;

/// <summary>
/// Service implementation for user operations
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepository<Topic> _topicRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IRepository<Topic> topicRepository,
        IRepository<Post> postRepository,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _topicRepository = topicRepository;
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        var topicCount = await _topicRepository.CountAsync(t => t.UserId == userId);
        var postCount = await _postRepository.CountAsync(p => p.UserId == userId);

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            DisplayName = user.DisplayName,
            Avatar = user.Avatar,
            ReputationPoints = user.ReputationPoints,
            Role = user.Role,
            JoinedDate = user.JoinedDate,
            IsBanned = user.IsBanned,
            TopicCount = topicCount,
            PostCount = postCount
        };
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return null;

        var topicCount = await _topicRepository.CountAsync(t => t.UserId == user.Id);
        var postCount = await _postRepository.CountAsync(p => p.UserId == user.Id);

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            DisplayName = user.DisplayName,
            Avatar = user.Avatar,
            ReputationPoints = user.ReputationPoints,
            Role = user.Role,
            JoinedDate = user.JoinedDate,
            IsBanned = user.IsBanned,
            TopicCount = topicCount,
            PostCount = postCount
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = _userManager.Users.ToList();
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var topicCount = await _topicRepository.CountAsync(t => t.UserId == user.Id);
            var postCount = await _postRepository.CountAsync(p => p.UserId == user.Id);

            userDtos.Add(new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                DisplayName = user.DisplayName,
                Avatar = user.Avatar,
                ReputationPoints = user.ReputationPoints,
                Role = user.Role,
                JoinedDate = user.JoinedDate,
                IsBanned = user.IsBanned,
                TopicCount = topicCount,
                PostCount = postCount
            });
        }

        return userDtos;
    }

    public async Task<UserDto> UpdateUserProfileAsync(string userId, UpdateUserProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        user.DisplayName = dto.DisplayName;
        user.Avatar = dto.Avatar;

        await _userManager.UpdateAsync(user);

        return (await GetUserByIdAsync(userId))!;
    }

    public async Task BanUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsBanned = true;
            await _userManager.UpdateAsync(user);
        }
    }

    public async Task UnbanUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.IsBanned = false;
            await _userManager.UpdateAsync(user);
        }
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        return _userManager.Users.Count();
    }
}
