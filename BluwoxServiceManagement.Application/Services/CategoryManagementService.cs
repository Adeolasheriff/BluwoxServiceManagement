using BluwoxServiceManagement.Application.DTOs.Request;
using BluwoxServiceManagement.Application.DTOs.Response;
using BluwoxServiceManagement.Application.Interfaces;
using BluwoxServiceManagement.Domain.Entities;
using BluwoxServiceManagement.Domain.Interface;


namespace BluwoxServiceManagement.Application.Services;

public class CategoryManagementService : ICategoryManagementService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryManagementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryResponse> GetCategoryByIdAsync(Guid id)
    {
        var categories = await _unitOfWork.Categories.FindAsync(c => c.Id == id);
        var category = categories.FirstOrDefault();

        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<List<CategoryResponse>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();

        return categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description
        }).ToList();
    }

    public async Task<CategoryResponse> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
    {
        var categories = await _unitOfWork.Categories.FindAsync(c => c.Id == id);
        var category = categories.FirstOrDefault();

        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        category.Name = request.Name;
        category.Description = request.Description;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var categories = await _unitOfWork.Categories.FindAsync(c => c.Id == id);
        var category = categories.FirstOrDefault();

        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        category.IsDeleted = true;

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}