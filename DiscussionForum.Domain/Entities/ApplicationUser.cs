using DiscussionForum.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace DiscussionForum.Domain.Entities;

/// <summary>
/// Represents a forum user with profile information
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? Avatar { get; set; }
    public int ReputationPoints { get; set; }
    public UserRole Role { get; set; }
    public DateTime JoinedDate { get; set; }
    public bool IsBanned { get; set; }
    
    // Navigation properties
    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
}
