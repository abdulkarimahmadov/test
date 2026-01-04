namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a forum category
/// </summary>
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedDate { get; set; }
    
    // Navigation properties
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
