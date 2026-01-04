using DiscussionForum.Domain.Enums;

namespace DiscussionForum.Application.DTOs;

public class TopicDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int ViewCount { get; set; }
    public TopicStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public bool IsPinned { get; set; }
    public bool IsLocked { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public int PostCount { get; set; }
}

public class CreateTopicDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public List<string> Tags { get; set; } = new();
}

public class UpdateTopicDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
