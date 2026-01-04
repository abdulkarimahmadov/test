using DiscussionForum.Domain.Enums;

namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a discussion topic/thread
/// </summary>
public class Topic
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
    
    // Foreign keys
    public int CategoryId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Category Category { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<TopicTag> TopicTags { get; set; } = new List<TopicTag>();
}
