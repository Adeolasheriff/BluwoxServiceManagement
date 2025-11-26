namespace BluwoxServiceManagement.Application.DTOs.Response;

public class ServiceResponse
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public List<CategoryResponse> Categories { get; set; } = new();
}

public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}