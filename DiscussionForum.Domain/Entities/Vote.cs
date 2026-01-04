namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a vote (upvote/downvote) on a post
/// </summary>
public class Vote
{
    public int Id { get; set; }
    public bool IsUpvote { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // Foreign keys
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    
    // Navigation properties
    public virtual Post Post { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}
