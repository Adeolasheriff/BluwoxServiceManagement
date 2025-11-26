namespace BluwoxServiceManagement.Domain.Entities;

public class ServiceEntity
{
    public Guid Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal BaseFare { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
}