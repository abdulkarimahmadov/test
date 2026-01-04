namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between topics and tags
/// </summary>
public class TopicTag
{
    public int TopicId { get; set; }
    public int TagId { get; set; }
    
    // Navigation properties
    public virtual Topic Topic { get; set; } = null!;
    public virtual Tag Tag { get; set; } = null!;
}
