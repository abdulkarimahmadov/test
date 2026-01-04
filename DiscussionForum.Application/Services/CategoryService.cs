using DiscussionForum.Application.DTOs;
using DiscussionForum.Application.Interfaces;
using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Interfaces;

namespace DiscussionForum.Application.Services;

/// <summary>
/// Service implementation for category operations
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Topic> _topicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        IRepository<Category> categoryRepository,
        IRepository<Topic> topicRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _topicRepository = topicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categoryDtos = new List<CategoryDto>();

        foreach (var category in categories.OrderBy(c => c.DisplayOrder))
        {
            var topicCount = await _topicRepository.CountAsync(t => t.CategoryId == category.Id);
            categoryDtos.Add(new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Icon = category.Icon,
                DisplayOrder = category.DisplayOrder,
                TopicCount = topicCount
            });
        }

        return categoryDtos;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null) return null;

        var topicCount = await _topicRepository.CountAsync(t => t.CategoryId == category.Id);
        
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            DisplayOrder = category.DisplayOrder,
            TopicCount = topicCount
        };
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
            DisplayOrder = dto.DisplayOrder,
            CreatedDate = DateTime.UtcNow
        };

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            DisplayOrder = category.DisplayOrder,
            TopicCount = 0
        };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(dto.Id);
        if (category == null)
            throw new InvalidOperationException($"Category with ID {dto.Id} not found");

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Icon = dto.Icon;
        category.DisplayOrder = dto.DisplayOrder;

        await _categoryRepository.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        var topicCount = await _topicRepository.CountAsync(t => t.CategoryId == category.Id);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Icon = category.Icon,
            DisplayOrder = category.DisplayOrder,
            TopicCount = topicCount
        };
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new InvalidOperationException($"Category with ID {id} not found");

        await _categoryRepository.DeleteAsync(category);
        await _unitOfWork.SaveChangesAsync();
    }
}
