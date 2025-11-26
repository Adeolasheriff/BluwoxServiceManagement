namespace BluwoxServiceManagement.Domain.Entities;
public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }

    public ICollection<ServiceCategory> ServiceCategories { get; set; } = new List<ServiceCategory>();
}