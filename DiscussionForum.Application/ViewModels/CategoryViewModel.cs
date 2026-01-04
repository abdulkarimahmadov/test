using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.ViewModels;

public class CategoryViewModel
{
    public CategoryDto Category { get; set; } = null!;
    public List<TopicDto> Topics { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
}
