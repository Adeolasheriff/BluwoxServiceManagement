using BluwoxServiceManagement.Domain.Entities;
using BluwoxServiceManagement.Domain.Interface;
using BluwoxServiceManagement.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace BluwoxServiceManagement.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private IRepository<ServiceEntity>? _services;
    private IRepository<Category>? _categories;
    private IRepository<ServiceCategory>? _serviceCategories;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IRepository<ServiceEntity> Services =>
        _services ??= new Repository<ServiceEntity>(_context);

    public IRepository<Category> Categories =>
        _categories ??= new Repository<Category>(_context);

    public IRepository<ServiceCategory> ServiceCategories =>
        _serviceCategories ??= new Repository<ServiceCategory>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}