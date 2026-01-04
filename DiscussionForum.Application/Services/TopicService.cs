using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Application.Services;

/// <summary>
/// Service implementation for topic operations
/// </summary>
public class TopicService : ITopicService
{
    private readonly IRepository<Topic> _topicRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TopicService(
        IRepository<Topic> topicRepository,
        IRepository<Post> postRepository,
        IUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await _topicRepository.GetAllAsync();
        return await MapToTopicDtos(topics);
    }

    public async Task<IEnumerable<TopicDto>> GetTopicsByCategoryAsync(int categoryId, int page = 1, int pageSize = 20)
    {
        var topics = await _topicRepository.FindAsync(t => t.CategoryId == categoryId);
        var paginatedTopics = topics
            .OrderByDescending(t => t.IsPinned)
            .ThenByDescending(t => t.LastActivityDate ?? t.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return await MapToTopicDtos(paginatedTopics);
    }

    public async Task<TopicDto?> GetTopicByIdAsync(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic == null) return null;

        var postCount = await _postRepository.CountAsync(p => p.TopicId == topic.Id);

        return new TopicDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            ViewCount = topic.ViewCount,
            Status = topic.Status,
            CreatedDate = topic.CreatedDate,
            LastActivityDate = topic.LastActivityDate,
            IsPinned = topic.IsPinned,
            IsLocked = topic.IsLocked,
            CategoryId = topic.CategoryId,
            CategoryName = topic.Category?.Name ?? "",
            UserId = topic.UserId,
            UserDisplayName = topic.User?.DisplayName ?? topic.User?.UserName ?? "",
            PostCount = postCount
        };
    }

    public async Task<TopicDto> CreateTopicAsync(CreateTopicDto dto, string userId)
    {
        var topic = new Topic
        {
            Title = dto.Title,
            Content = dto.Content,
            CategoryId = dto.CategoryId,
            UserId = userId,
            CreatedDate = DateTime.UtcNow,
            LastActivityDate = DateTime.UtcNow,
            ViewCount = 0,
            IsPinned = false,
            IsLocked = false,
            Status = Domain.Enums.TopicStatus.Open
        };

        await _topicRepository.AddAsync(topic);
        await _unitOfWork.SaveChangesAsync();

        return (await GetTopicByIdAsync(topic.Id))!;
    }

    public async Task<TopicDto> UpdateTopicAsync(UpdateTopicDto dto)
    {
        var topic = await _topicRepository.GetByIdAsync(dto.Id);
        if (topic == null)
            throw new InvalidOperationException($"Topic with ID {dto.Id} not found");

        topic.Title = dto.Title;
        topic.Content = dto.Content;
        topic.CategoryId = dto.CategoryId;

        await _topicRepository.UpdateAsync(topic);
        await _unitOfWork.SaveChangesAsync();

        return (await GetTopicByIdAsync(topic.Id))!;
    }

    public async Task DeleteTopicAsync(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        if (topic == null)
            throw new InvalidOperationException($"Topic with ID {id} not found");

        await _topicRepository.DeleteAsync(topic);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task IncrementViewCountAsync(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic != null)
        {
            topic.ViewCount++;
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<TopicDto>> SearchTopicsAsync(string searchTerm)
    {
        var topics = await _topicRepository.FindAsync(t => 
            t.Title.Contains(searchTerm) || t.Content.Contains(searchTerm));
        return await MapToTopicDtos(topics);
    }

    public async Task<int> GetTopicCountByCategoryAsync(int categoryId)
    {
        return await _topicRepository.CountAsync(t => t.CategoryId == categoryId);
    }

    public async Task PinTopicAsync(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic != null)
        {
            topic.IsPinned = true;
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task UnpinTopicAsync(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic != null)
        {
            topic.IsPinned = false;
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task LockTopicAsync(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic != null)
        {
            topic.IsLocked = true;
            topic.Status = Domain.Enums.TopicStatus.Closed;
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task UnlockTopicAsync(int topicId)
    {
        var topic = await _topicRepository.GetByIdAsync(topicId);
        if (topic != null)
        {
            topic.IsLocked = false;
            topic.Status = Domain.Enums.TopicStatus.Open;
            await _topicRepository.UpdateAsync(topic);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private async Task<List<TopicDto>> MapToTopicDtos(IEnumerable<Topic> topics)
    {
        var topicDtos = new List<TopicDto>();
        foreach (var topic in topics)
        {
            var postCount = await _postRepository.CountAsync(p => p.TopicId == topic.Id);
            topicDtos.Add(new TopicDto
            {
                Id = topic.Id,
                Title = topic.Title,
                Content = topic.Content,
                ViewCount = topic.ViewCount,
                Status = topic.Status,
                CreatedDate = topic.CreatedDate,
                LastActivityDate = topic.LastActivityDate,
                IsPinned = topic.IsPinned,
                IsLocked = topic.IsLocked,
                CategoryId = topic.CategoryId,
                CategoryName = topic.Category?.Name ?? "",
                UserId = topic.UserId,
                UserDisplayName = topic.User?.DisplayName ?? topic.User?.UserName ?? "",
                PostCount = postCount
            });
        }
        return topicDtos;
    }
}
