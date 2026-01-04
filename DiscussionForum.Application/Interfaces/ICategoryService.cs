using DiscussionForum.Application.DTOs;

namespace DiscussionForum.Application.Interfaces;

/// <summary>
/// Service interface for category operations
/// </summary>
public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto?> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto);
    Task DeleteCategoryAsync(int id);
}
