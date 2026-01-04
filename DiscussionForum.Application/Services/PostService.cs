using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Interfaces;

namespace DiscussionForum.Application.Services;

/// <summary>
/// Service implementation for post operations
/// </summary>
public class PostService : IPostService
{
    private readonly IRepository<Post> _postRepository;
    private readonly IRepository<Topic> _topicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PostService(
        IRepository<Post> postRepository,
        IRepository<Topic> topicRepository,
        IUnitOfWork unitOfWork)
    {
        _postRepository = postRepository;
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PostDto>> GetPostsByTopicAsync(int topicId, int page = 1, int pageSize = 20)
    {
        var posts = await _postRepository.FindAsync(p => p.TopicId == topicId);
        var paginatedPosts = posts
            .OrderBy(p => p.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        return paginatedPosts.Select(p => new PostDto
        {
            Id = p.Id,
            Content = p.Content,
            CreatedDate = p.CreatedDate,
            EditedDate = p.EditedDate,
            IsEdited = p.IsEdited,
            VoteCount = p.VoteCount,
            TopicId = p.TopicId,
            UserId = p.UserId,
            UserDisplayName = p.User?.DisplayName ?? p.User?.UserName ?? "",
            UserAvatar = p.User?.Avatar,
            QuotedPostId = p.QuotedPostId
        }).ToList();
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null) return null;

        return new PostDto
        {
            Id = post.Id,
            Content = post.Content,
            CreatedDate = post.CreatedDate,
            EditedDate = post.EditedDate,
            IsEdited = post.IsEdited,
            VoteCount = post.VoteCount,
            TopicId = post.TopicId,
            UserId = post.UserId,
            UserDisplayName = post.User?.DisplayName ?? post.User?.UserName ?? "",
            UserAvatar = post.User?.Avatar,
            QuotedPostId = post.QuotedPostId
        };
    }

    public async Task<PostDto> CreatePostAsync(CreatePostDto dto, string userId)
    {
        var post = new Post
        {
            Content = dto.Content,
            TopicId = dto.TopicId,
            UserId = userId,
            CreatedDate = DateTime.UtcNow,
            QuotedPostId = dto.QuotedPostId,
            VoteCount = 0,
            IsEdited = false
        };

        await _postRepository.AddAsync(post);

        // Update topic's last activity date
        var topic = await _topicRepository.GetByIdAsync(dto.TopicId);
        if (topic != null)
        {
            topic.LastActivityDate = DateTime.UtcNow;
            await _topicRepository.UpdateAsync(topic);
        }

        await _unitOfWork.SaveChangesAsync();

        return (await GetPostByIdAsync(post.Id))!;
    }

    public async Task<PostDto> UpdatePostAsync(UpdatePostDto dto)
    {
        var post = await _postRepository.GetByIdAsync(dto.Id);
        if (post == null)
            throw new InvalidOperationException($"Post with ID {dto.Id} not found");

        post.Content = dto.Content;
        post.EditedDate = DateTime.UtcNow;
        post.IsEdited = true;

        await _postRepository.UpdateAsync(post);
        await _unitOfWork.SaveChangesAsync();

        return (await GetPostByIdAsync(post.Id))!;
    }

    public async Task DeletePostAsync(int id)
    {
        var post = await _postRepository.GetByIdAsync(id);
        if (post == null)
            throw new InvalidOperationException($"Post with ID {id} not found");

        await _postRepository.DeleteAsync(post);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<int> GetPostCountByTopicAsync(int topicId)
    {
        return await _postRepository.CountAsync(p => p.TopicId == topicId);
    }
}
