using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscussionForum.Web.Controllers;

public class PostController : Controller
{
    private readonly IPostService _postService;
    private readonly IVoteService _voteService;

    public PostController(IPostService postService, IVoteService voteService)
    {
        _postService = postService;
        _voteService = voteService;
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostDto dto)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var post = await _postService.CreatePostAsync(dto, userId);
            return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = dto.TopicId });
        }

        return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = dto.TopicId });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            return Forbid();

        var dto = new UpdatePostDto
        {
            Id = post.Id,
            Content = post.Content
        };

        return View(dto);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdatePostDto dto)
    {
        if (ModelState.IsValid)
        {
            var post = await _postService.GetPostByIdAsync(dto.Id);
            if (post == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
                return Forbid();

            await _postService.UpdatePostAsync(dto);
            return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = post.TopicId });
        }

        return View(dto);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (post.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            return Forbid();

        var topicId = post.TopicId;
        await _postService.DeletePostAsync(id);
        return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = topicId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Upvote(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        await _voteService.UpvotePostAsync(id, userId);
        
        var post = await _postService.GetPostByIdAsync(id);
        return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = post?.TopicId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Downvote(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();

        await _voteService.DownvotePostAsync(id, userId);
        
        var post = await _postService.GetPostByIdAsync(id);
        return RedirectToAction(nameof(TopicController.Details), "Topic", new { id = post?.TopicId });
    }
}
