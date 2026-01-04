namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a tag that can be applied to topics
/// </summary>
public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    
    // Navigation properties
    public virtual ICollection<TopicTag> TopicTags { get; set; } = new List<TopicTag>();
}
