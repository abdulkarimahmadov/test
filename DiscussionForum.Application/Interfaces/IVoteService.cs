namespace DiscussionForum.Application.Interfaces;

/// <summary>
/// Service interface for voting operations
/// </summary>
public interface IVoteService
{
    Task<bool> UpvotePostAsync(int postId, string userId);
    Task<bool> DownvotePostAsync(int postId, string userId);
    Task<bool> RemoveVoteAsync(int postId, string userId);
    Task<int> GetVoteCountForPostAsync(int postId);
    Task<bool> HasUserVotedAsync(int postId, string userId);
}
