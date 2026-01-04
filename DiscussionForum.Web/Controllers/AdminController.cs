using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;
    private readonly ITopicService _topicService;

    public AdminController(
        ICategoryService categoryService,
        IUserService userService,
        ITopicService topicService)
    {
        _categoryService = categoryService;
        _userService = userService;
        _topicService = topicService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        var categories = await _categoryService.GetAllCategoriesAsync();
        var topics = await _topicService.GetAllTopicsAsync();

        ViewBag.TotalUsers = users.Count();
        ViewBag.TotalCategories = categories.Count();
        ViewBag.TotalTopics = topics.Count();

        return View();
    }

    // Category Management
    public async Task<IActionResult> Categories()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.CreateCategoryAsync(dto);
            TempData["SuccessMessage"] = "Category created successfully!";
            return RedirectToAction(nameof(Categories));
        }

        return View(dto);
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        var dto = new UpdateCategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            DisplayOrder = category.DisplayOrder
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(UpdateCategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.UpdateCategoryAsync(dto);
            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction(nameof(Categories));
        }

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["SuccessMessage"] = "Category deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
        }

        return RedirectToAction(nameof(Categories));
    }

    // User Management
    public async Task<IActionResult> Users()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BanUser(string id)
    {
        await _userService.BanUserAsync(id);
        TempData["SuccessMessage"] = "User banned successfully!";
        return RedirectToAction(nameof(Users));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnbanUser(string id)
    {
        await _userService.UnbanUserAsync(id);
        TempData["SuccessMessage"] = "User unbanned successfully!";
        return RedirectToAction(nameof(Users));
    }
}
