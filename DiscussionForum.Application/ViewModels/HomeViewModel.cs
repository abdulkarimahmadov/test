using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.ViewModels;

public class HomeViewModel
{
    public List<CategoryDto> Categories { get; set; } = new();
    public int TotalUsers { get; set; }
    public int TotalTopics { get; set; }
    public int TotalPosts { get; set; }
    public List<TopicDto> RecentTopics { get; set; } = new();
}
