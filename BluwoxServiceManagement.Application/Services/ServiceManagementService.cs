
using BluwoxServiceManagement.Application.DTOs.Request;
using BluwoxServiceManagement.Application.DTOs.Response;
using BluwoxServiceManagement.Application.Interfaces;
using BluwoxServiceManagement.Domain.Entities;
using BluwoxServiceManagement.Domain.Interface;


namespace BluwoxServiceManagement.Application.Services;

public class ServiceManagementService : IServiceManagementService
{
    private readonly IUnitOfWork _unitOfWork;

    public ServiceManagementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse> CreateServiceAsync(CreateServiceRequest request)
    {
        var categories = await _unitOfWork.Categories.FindAsync(c => request.CategoryIds.Contains(c.Id));
        var categoryList = categories.ToList();

        if (categoryList.Count != request.CategoryIds.Count)
            throw new ArgumentException("One or more category IDs are invalid");

        var service = new ServiceEntity
        {
            Id = Guid.NewGuid(),
            ServiceName = request.ServiceName,
            BaseFare = request.BaseFare,
            IsActive = request.IsActive,
            CreatedDate = DateTime.UtcNow
        };

        await _unitOfWork.Services.AddAsync(service);

        foreach (var categoryId in request.CategoryIds)
        {
            var serviceCategory = new ServiceCategory
            {
                ServiceId = service.Id,
                CategoryId = categoryId,
                AssignedDate = DateTime.UtcNow
            };
            await _unitOfWork.ServiceCategories.AddAsync(serviceCategory);
        }

        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse
        {
            Id = service.Id,
            ServiceName = service.ServiceName,
            BaseFare = service.BaseFare,
            IsActive = service.IsActive,
            CreatedDate = service.CreatedDate,
            Categories = categoryList.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList()
        };
    }

    public async Task<ServiceResponse> GetServiceByIdAsync(Guid id)
    {
        var services = await _unitOfWork.Services.FindAsync(s => s.Id == id);
        var service = services.FirstOrDefault();

        if (service == null)
            throw new KeyNotFoundException($"Service with ID {id} not found");

        var serviceCategories = await _unitOfWork.ServiceCategories.FindAsync(sc => sc.ServiceId == id);
        var categoryIds = serviceCategories.Select(sc => sc.CategoryId).ToList();
        var categories = await _unitOfWork.Categories.FindAsync(c => categoryIds.Contains(c.Id));

        return new ServiceResponse
        {
            Id = service.Id,
            ServiceName = service.ServiceName,
            BaseFare = service.BaseFare,
            IsActive = service.IsActive,
            CreatedDate = service.CreatedDate,
            ModifiedDate = service.ModifiedDate,
            Categories = categories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList()
        };
    }

    public async Task<PagedResponse<ServiceResponse>> GetAllServicesAsync(int pageNumber, int pageSize)
    {
        var totalRecords = await _unitOfWork.Services.CountAsync();
        var services = await _unitOfWork.Services.GetPagedAsync(pageNumber, pageSize);
        var serviceList = services.ToList();

        var serviceResponses = new List<ServiceResponse>();

        foreach (var service in serviceList)
        {
            var serviceCategories = await _unitOfWork.ServiceCategories.FindAsync(sc => sc.ServiceId == service.Id);
            var categoryIds = serviceCategories.Select(sc => sc.CategoryId).ToList();
            var categories = categoryIds.Any()
                ? await _unitOfWork.Categories.FindAsync(c => categoryIds.Contains(c.Id))
                : Enumerable.Empty<Category>();

            serviceResponses.Add(new ServiceResponse
            {
                Id = service.Id,
                ServiceName = service.ServiceName,
                BaseFare = service.BaseFare,
                IsActive = service.IsActive,
                CreatedDate = service.CreatedDate,
                ModifiedDate = service.ModifiedDate,
                Categories = categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList()
            });
        }

        return new PagedResponse<ServiceResponse>(serviceResponses, pageNumber, pageSize, totalRecords);
    }

    public async Task<ServiceResponse> UpdateServiceAsync(Guid id, UpdateServiceRequest request)
    {
        var services = await _unitOfWork.Services.FindAsync(s => s.Id == id);
        var service = services.FirstOrDefault();

        if (service == null)
            throw new KeyNotFoundException($"Service with ID {id} not found");

        var categories = await _unitOfWork.Categories.FindAsync(c => request.CategoryIds.Contains(c.Id));
        var categoryList = categories.ToList();

        if (categoryList.Count != request.CategoryIds.Count)
            throw new ArgumentException("One or more category IDs are invalid");

        service.ServiceName = request.ServiceName;
        service.BaseFare = request.BaseFare;
        service.IsActive = request.IsActive;
        service.ModifiedDate = DateTime.UtcNow;

        await _unitOfWork.Services.UpdateAsync(service);

        var existingCategories = await _unitOfWork.ServiceCategories.FindAsync(sc => sc.ServiceId == id);
        foreach (var existingCategory in existingCategories)
        {
            await _unitOfWork.ServiceCategories.DeleteAsync(existingCategory);
        }

        foreach (var categoryId in request.CategoryIds)
        {
            var serviceCategory = new ServiceCategory
            {
                ServiceId = service.Id,
                CategoryId = categoryId,
                AssignedDate = DateTime.UtcNow
            };
            await _unitOfWork.ServiceCategories.AddAsync(serviceCategory);
        }

        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse
        {
            Id = service.Id,
            ServiceName = service.ServiceName,
            BaseFare = service.BaseFare,
            IsActive = service.IsActive,
            CreatedDate = service.CreatedDate,
            ModifiedDate = service.ModifiedDate,
            Categories = categoryList.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList()
        };
    }

    public async Task<bool> DeleteServiceAsync(Guid id)
    {
        var services = await _unitOfWork.Services.FindAsync(s => s.Id == id);
        var service = services.FirstOrDefault();

        if (service == null)
            throw new KeyNotFoundException($"Service with ID {id} not found");

        service.IsDeleted = true;
        service.ModifiedDate = DateTime.UtcNow;

        await _unitOfWork.Services.UpdateAsync(service);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}