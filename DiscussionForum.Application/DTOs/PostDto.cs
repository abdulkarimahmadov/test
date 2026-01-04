namespace DiscussionForum.Application.DTOs;

public class PostDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime? EditedDate { get; set; }
    public bool IsEdited { get; set; }
    public int VoteCount { get; set; }
    public int TopicId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public string? UserAvatar { get; set; }
    public int? QuotedPostId { get; set; }
}

public class CreatePostDto
{
    public string Content { get; set; } = string.Empty;
    public int TopicId { get; set; }
    public int? QuotedPostId { get; set; }
}

public class UpdatePostDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
}
