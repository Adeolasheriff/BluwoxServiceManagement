using BluwoxServiceManagement.Application.DTOs.Request;
using BluwoxServiceManagement.Application.DTOs.Response;

namespace BluwoxServiceManagement.Application.Interfaces;

public interface ICategoryManagementService
{
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request);
    Task<CategoryResponse> GetCategoryByIdAsync(Guid id);
    Task<List<CategoryResponse>> GetAllCategoriesAsync();
    Task<CategoryResponse> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
    Task<bool> DeleteCategoryAsync(Guid id);
}