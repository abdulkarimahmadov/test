using DiscussionForum.Application.Interfaces;
using DiscussionForum.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionForum.Web.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ITopicService _topicService;

    public CategoryController(ICategoryService categoryService, ITopicService topicService)
    {
        _categoryService = categoryService;
        _topicService = topicService;
    }

    public async Task<IActionResult> Index(int id, int page = 1, int pageSize = 20)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        var topics = await _topicService.GetTopicsByCategoryAsync(id, page, pageSize);
        var totalTopics = await _topicService.GetTopicCountByCategoryAsync(id);
        var totalPages = (int)Math.Ceiling(totalTopics / (double)pageSize);

        var viewModel = new CategoryViewModel
        {
            Category = category,
            Topics = topics.ToList(),
            CurrentPage = page,
            TotalPages = totalPages,
            PageSize = pageSize
        };

        return View(viewModel);
    }
}
