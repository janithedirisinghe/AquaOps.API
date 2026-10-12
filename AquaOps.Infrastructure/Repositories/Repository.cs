using System.Linq.Expressions;
using AquaOps.Domain.Interfaces;
using AquaOps.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaOps.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AquaOpsDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AquaOpsDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _dbSet.FindAsync(new object[] { id }, ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ToListAsync(ct);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.AnyAsync(predicate, ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        return entity;
    }

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AquaOpsDbContext _context;
    private bool _disposed;

    public UnitOfWork(AquaOpsDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
    }
}
