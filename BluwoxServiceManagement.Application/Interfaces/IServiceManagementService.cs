using BluwoxServiceManagement.Application.DTOs.Request;
using BluwoxServiceManagement.Application.DTOs.Response;

namespace BluwoxServiceManagement.Application.Interfaces;

public interface IServiceManagementService
{
    Task<ServiceResponse> CreateServiceAsync(CreateServiceRequest request);
    Task<ServiceResponse> GetServiceByIdAsync(Guid id);
    Task<PagedResponse<ServiceResponse>> GetAllServicesAsync(int pageNumber, int pageSize);
    Task<ServiceResponse> UpdateServiceAsync(Guid id, UpdateServiceRequest request);
    Task<bool> DeleteServiceAsync(Guid id);
}