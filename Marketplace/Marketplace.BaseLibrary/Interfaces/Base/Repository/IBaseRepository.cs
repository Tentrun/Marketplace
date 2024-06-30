namespace Marketplace.BaseLibrary.Interfaces.Base.Repository;

public interface IBaseRepository<T> where T : class
{
    public Task<bool> Create(T entity);
    public Task<bool> CreateAsync(T entity);
    public Task<bool> Update(T entity);
    public Task<bool> UpdateAsync(T entity);
    public Task<bool> Remove(T entity);
    public Task<bool> RemoveAsync(T entity);
}