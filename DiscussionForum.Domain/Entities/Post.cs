namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a post/reply in a topic
/// </summary>
public class Post
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? EditedDate { get; set; }
    public bool IsEdited { get; set; }
    public int VoteCount { get; set; }
    
    // Foreign keys
    public int TopicId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? QuotedPostId { get; set; }
    
    // Navigation properties
    public virtual Topic Topic { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Post? QuotedPost { get; set; }
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
