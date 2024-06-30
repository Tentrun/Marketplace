using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.BaseLibrary.Interfaces.Base;

public abstract class BaseRepository<T> : IBaseRepository<T>
    where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly DbContext _dbContext;

    protected BaseRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }
    
    public virtual Task<bool> Create(T entity)
    {
        _dbSet.Add(entity);
        _dbContext.SaveChanges();

        return Task.FromResult(true);
    }

    public virtual async Task<bool> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public virtual Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        _dbContext.SaveChanges();

        return Task.FromResult(true);
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public virtual Task<bool> Remove(T entity)
    {
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
        
        return Task.FromResult(true);
    }

    public virtual async Task<bool> RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();
        
        return true;
    }
}