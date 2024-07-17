using System.Linq.Expressions;

namespace Marketplace.BaseLibrary.Interfaces.Base.Repository;

/// <summary>
/// Базовый репозиторий для наследования
/// </summary>
/// <typeparam name="T">Тип сущности репозитория</typeparam>
public interface IBaseRepository<T> : IDisposable, IAsyncDisposable where T : class
{
    public Task<bool> Create(T entity);
    public Task<bool> CreateAsync(T entity);
    public Task<bool> Update(T entity);
    public Task<bool> UpdateAsync(T entity);
    public Task<bool> Remove(T entity);
    public Task<bool> RemoveAsync(T entity);
    public Task<T?> GetFirstOrDefaultWithPredicate(Func<T, bool> predicate,
        params Expression<Func<T, object>>[] includeProperties);
    public Task<List<T?>?> GetListWithPredicate(Func<T, bool> predicate,
        params Expression<Func<T, object>>[] includeProperties);
}