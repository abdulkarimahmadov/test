using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DiscussionForum.Web.Controllers;

public class TopicController : Controller
{
    private readonly ITopicService _topicService;
    private readonly IPostService _postService;
    private readonly ICategoryService _categoryService;

    public TopicController(
        ITopicService topicService,
        IPostService postService,
        ICategoryService categoryService)
    {
        _topicService = topicService;
        _postService = postService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Details(int id, int page = 1, int pageSize = 20)
    {
        var topic = await _topicService.GetTopicByIdAsync(id);
        if (topic == null)
            return NotFound();

        await _topicService.IncrementViewCountAsync(id);

        var posts = await _postService.GetPostsByTopicAsync(id, page, pageSize);
        var totalPosts = await _postService.GetPostCountByTopicAsync(id);
        var totalPages = (int)Math.Ceiling(totalPosts / (double)pageSize);

        var viewModel = new TopicDetailsViewModel
        {
            Topic = topic,
            Posts = posts.ToList(),
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(viewModel);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(int categoryId)
    {
        var category = await _categoryService.GetCategoryByIdAsync(categoryId);
        if (category == null)
            return NotFound();

        ViewBag.Category = category;
        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTopicDto dto)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var topic = await _topicService.CreateTopicAsync(dto, userId);
            return RedirectToAction(nameof(Details), new { id = topic.Id });
        }

        var category = await _categoryService.GetCategoryByIdAsync(dto.CategoryId);
        ViewBag.Category = category;
        return View(dto);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var topic = await _topicService.GetTopicByIdAsync(id);
        if (topic == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (topic.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            return Forbid();

        var dto = new UpdateTopicDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Content = topic.Content,
            CategoryId = topic.CategoryId
        };

        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Categories = categories;
        return View(dto);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateTopicDto dto)
    {
        if (ModelState.IsValid)
        {
            var topic = await _topicService.GetTopicByIdAsync(dto.Id);
            if (topic == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (topic.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
                return Forbid();

            await _topicService.UpdateTopicAsync(dto);
            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        var categories = await _categoryService.GetAllCategoriesAsync();
        ViewBag.Categories = categories;
        return View(dto);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var topic = await _topicService.GetTopicByIdAsync(id);
        if (topic == null)
            return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (topic.UserId != userId && !User.IsInRole("Admin") && !User.IsInRole("Moderator"))
            return Forbid();

        var categoryId = topic.CategoryId;
        await _topicService.DeleteTopicAsync(id);
        return RedirectToAction(nameof(CategoryController.Index), "Category", new { id = categoryId });
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return RedirectToAction(nameof(HomeController.Index), "Home");

        var topics = await _topicService.SearchTopicsAsync(q);
        ViewBag.SearchTerm = q;
        return View(topics);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Pin(int id)
    {
        await _topicService.PinTopicAsync(id);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Unpin(int id)
    {
        await _topicService.UnpinTopicAsync(id);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Lock(int id)
    {
        await _topicService.LockTopicAsync(id);
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Moderator")]
    public async Task<IActionResult> Unlock(int id)
    {
        await _topicService.UnlockTopicAsync(id);
        return RedirectToAction(nameof(Details), new { id });
    }
}
