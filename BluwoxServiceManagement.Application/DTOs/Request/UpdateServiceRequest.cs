
namespace BluwoxServiceManagement.Application.DTOs.Request;

public class UpdateServiceRequest
{
    public string ServiceName { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public bool IsActive { get; set; }
    public List<Guid> CategoryIds { get; set; } = new();
}