using System.Diagnostics;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DiscussionForum.Web.Models;

namespace DiscussionForum.Web.Controllers;

public class HomeController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly ITopicService _topicService;
    private readonly IUserService _userService;

    public HomeController(
        ICategoryService categoryService,
        ITopicService topicService,
        IUserService userService)
    {
        _categoryService = categoryService;
        _topicService = topicService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        var topics = await _topicService.GetAllTopicsAsync();
        var recentTopics = topics.OrderByDescending(t => t.CreatedDate).Take(10).ToList();

        var viewModel = new HomeViewModel
        {
            Categories = categories.ToList(),
            RecentTopics = recentTopics,
            TotalUsers = await _userService.GetTotalUsersCountAsync(),
            TotalTopics = topics.Count()
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
