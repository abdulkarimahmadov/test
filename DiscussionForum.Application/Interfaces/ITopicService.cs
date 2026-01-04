using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.Interfaces;

/// <summary>
/// Service interface for topic operations
/// </summary>
public interface ITopicService
{
    Task<IEnumerable<TopicDto>> GetAllTopicsAsync();
    Task<IEnumerable<TopicDto>> GetTopicsByCategoryAsync(int categoryId, int page = 1, int pageSize = 20);
    Task<TopicDto?> GetTopicByIdAsync(int id);
    Task<TopicDto> CreateTopicAsync(CreateTopicDto dto, string userId);
    Task<TopicDto> UpdateTopicAsync(UpdateTopicDto dto);
    Task DeleteTopicAsync(int id);
    Task IncrementViewCountAsync(int topicId);
    Task<IEnumerable<TopicDto>> SearchTopicsAsync(string searchTerm);
    Task<int> GetTopicCountByCategoryAsync(int categoryId);
    Task PinTopicAsync(int topicId);
    Task UnpinTopicAsync(int topicId);
    Task LockTopicAsync(int topicId);
    Task UnlockTopicAsync(int topicId);
}
