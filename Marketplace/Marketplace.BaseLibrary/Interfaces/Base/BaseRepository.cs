using System.Linq.Expressions;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.BaseLibrary.Interfaces.Base;

/// <summary>
/// Базовый репозиторий для наследования, с базовой логикой, для последующего переопределения, или использования как есть
/// </summary>
/// <typeparam name="T">Тип сущности репозитория</typeparam>
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
    
    public Task<List<T?>?> GetListWithPredicate(Func<T, bool> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query =  GetWithPredicate(includeProperties);
        return Task.FromResult(query.AsEnumerable().Where(predicate).ToList());
    }
 
    public Task<T?> GetFirstOrDefaultWithPredicate(Func<T,bool> predicate, 
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query =  GetWithPredicate(includeProperties);
        return Task.FromResult(query.AsEnumerable().FirstOrDefault(predicate));
    }
    
    private IQueryable<T?> GetWithPredicate(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _dbSet;
        return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}