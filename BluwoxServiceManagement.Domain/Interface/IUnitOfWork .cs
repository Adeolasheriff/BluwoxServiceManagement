using BluwoxServiceManagement.Domain.Entities;

namespace BluwoxServiceManagement.Domain.Interface;

public interface IUnitOfWork : IDisposable
{
    IRepository<ServiceEntity> Services { get; }
    IRepository<Category> Categories { get; }
    IRepository<ServiceCategory> ServiceCategories { get; }
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}