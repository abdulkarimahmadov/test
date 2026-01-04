using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.Interfaces;

/// <summary>
/// Service interface for post operations
/// </summary>
public interface IPostService
{
    Task<IEnumerable<PostDto>> GetPostsByTopicAsync(int topicId, int page = 1, int pageSize = 20);
    Task<PostDto?> GetPostByIdAsync(int id);
    Task<PostDto> CreatePostAsync(CreatePostDto dto, string userId);
    Task<PostDto> UpdatePostAsync(UpdatePostDto dto);
    Task DeletePostAsync(int id);
    Task<int> GetPostCountByTopicAsync(int topicId);
}
