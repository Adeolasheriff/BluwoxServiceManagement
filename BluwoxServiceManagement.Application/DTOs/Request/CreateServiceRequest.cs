namespace BluwoxServiceManagement.Application.DTOs.Request;

public class CreateServiceRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public bool IsActive { get; set; } = true;
    public List<Guid> CategoryIds { get; set; } = new();
}