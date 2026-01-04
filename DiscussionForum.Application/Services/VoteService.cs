using DiscussionForum.Application.Interfaces;
using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Interfaces;

namespace DiscussionForum.Application.Services;

/// <summary>
/// Service implementation for voting operations
/// </summary>
public class VoteService : IVoteService
{
    private readonly IRepository<Vote> _voteRepository;
    private readonly IRepository<Post> _postRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VoteService(
        IRepository<Vote> voteRepository,
        IRepository<Post> postRepository,
        IUnitOfWork unitOfWork)
    {
        _voteRepository = voteRepository;
        _postRepository = postRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> UpvotePostAsync(int postId, string userId)
    {
        var existingVote = (await _voteRepository.FindAsync(v => v.PostId == postId && v.UserId == userId))
            .FirstOrDefault();

        if (existingVote != null)
        {
            if (existingVote.IsUpvote)
                return false; // Already upvoted

            existingVote.IsUpvote = true;
            await _voteRepository.UpdateAsync(existingVote);
        }
        else
        {
            var vote = new Vote
            {
                PostId = postId,
                UserId = userId,
                IsUpvote = true,
                CreatedDate = DateTime.UtcNow
            };
            await _voteRepository.AddAsync(vote);
        }

        await UpdatePostVoteCount(postId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DownvotePostAsync(int postId, string userId)
    {
        var existingVote = (await _voteRepository.FindAsync(v => v.PostId == postId && v.UserId == userId))
            .FirstOrDefault();

        if (existingVote != null)
        {
            if (!existingVote.IsUpvote)
                return false; // Already downvoted

            existingVote.IsUpvote = false;
            await _voteRepository.UpdateAsync(existingVote);
        }
        else
        {
            var vote = new Vote
            {
                PostId = postId,
                UserId = userId,
                IsUpvote = false,
                CreatedDate = DateTime.UtcNow
            };
            await _voteRepository.AddAsync(vote);
        }

        await UpdatePostVoteCount(postId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveVoteAsync(int postId, string userId)
    {
        var vote = (await _voteRepository.FindAsync(v => v.PostId == postId && v.UserId == userId))
            .FirstOrDefault();

        if (vote == null)
            return false;

        await _voteRepository.DeleteAsync(vote);
        await UpdatePostVoteCount(postId);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetVoteCountForPostAsync(int postId)
    {
        var votes = await _voteRepository.FindAsync(v => v.PostId == postId);
        return votes.Sum(v => v.IsUpvote ? 1 : -1);
    }

    public async Task<bool> HasUserVotedAsync(int postId, string userId)
    {
        var vote = (await _voteRepository.FindAsync(v => v.PostId == postId && v.UserId == userId))
            .FirstOrDefault();
        return vote != null;
    }

    private async Task UpdatePostVoteCount(int postId)
    {
        var post = await _postRepository.GetByIdAsync(postId);
        if (post != null)
        {
            var voteCount = await GetVoteCountForPostAsync(postId);
            post.VoteCount = voteCount;
            await _postRepository.UpdateAsync(post);
        }
    }
}
