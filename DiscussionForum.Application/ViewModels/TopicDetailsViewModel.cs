using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.ViewModels;

public class TopicDetailsViewModel
{
    public TopicDto Topic { get; set; } = null!;
    public List<PostDto> Posts { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}
