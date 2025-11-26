namespace BluwoxServiceManagement.Domain.Entities;

public class ServiceCategory
{
    public Guid ServiceId { get; set; }
    public ServiceEntity Service { get; set; } = null!;

    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public DateTime AssignedDate { get; set; }
}

