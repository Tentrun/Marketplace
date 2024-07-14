using System.Linq.Expressions;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.BaseLibrary.Implementation;

/// <summary>
/// Базовый репозиторий для наследования, с базовой логикой, для последующего переопределения, или использования как есть
/// </summary>
/// <typeparam name="T">Тип сущности репозитория</typeparam>
/// <typeparam name="C">Тип контекста фабрики (check remarks)</typeparam>
/// <remarks>К сожалению, EF Core не умеет приводить IDbContextFactory от базового контекста к строго типизированному, даже если тот является наследником базового.</remarks>
/// <remarks> По этому придется использовать такой костыль, до лучших времен</remarks>
public abstract class BaseRepository<T, C> : IBaseRepository<T>
    where T : class 
    where C : DbContext
{
    private readonly DbSet<T> _dbSet;
    private readonly DbContext _dbContext;

    protected BaseRepository(IDbContextFactory<C> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
        _dbSet = _dbContext.Set<T>();
    }
    
    /// <summary>
    /// Создает сущность в базе данных
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для создания</param>
    /// <returns>Результат создания</returns>
    public virtual Task<bool> Create(T entity)
    {
        _dbSet.Add(entity);
        _dbContext.SaveChanges();

        return Task.FromResult(true);
    }

    /// <summary>
    /// Создает сущность в базе данных в ассинхронном варианте
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для создания</param>
    /// <returns>Результат создания</returns>
    public virtual async Task<bool> CreateAsync(T entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Обновляет сущность в базе данных
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для обновления</param>
    /// <returns>Результат обновления</returns>
    public virtual Task<bool> Update(T entity)
    {
        _dbSet.Update(entity);
        _dbContext.SaveChanges();

        return Task.FromResult(true);
    }

    /// <summary>
    /// Обновляет сущность в базе данных в ассинхронном варианте
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для обновления</param>
    /// <returns>Результат обновления</returns>
    public virtual async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    
    /// <summary>
    /// Удаляет сущность в базе данных
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для удаления</param>
    /// <returns>Результат удаления</returns>
    public virtual Task<bool> Remove(T entity)
    {
        _dbSet.Remove(entity);
        _dbContext.SaveChanges();
        
        return Task.FromResult(true);
    }

    /// <summary>
    /// Удаляет сущность в базе данных в ассинхронном варианте
    /// </summary>
    /// <param name="entity">Явно типизированная сущность для удаления</param>
    /// <returns>Результат удаления</returns>
    public virtual async Task<bool> RemoveAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    
    /// <summary>
    /// Получает коллекцию объектов из БД по переданному в метод предикату
    /// </summary>
    /// <param name="predicate">Предикат поиска</param>
    /// <param name="includeProperties">Включаемые параметры</param>
    /// <returns>Коллекция объектов по переданному фильтру</returns>
    public Task<List<T?>?> GetListWithPredicate(Func<T, bool> predicate,
        params Expression<Func<T, object>>[] includeProperties)
    {
        var query =  GetWithPredicate(includeProperties);
        return Task.FromResult(query.AsEnumerable().Where(predicate).ToList());
    }
 
    /// <summary>
    /// Получает объект из БД по переданному в метод предикату
    /// </summary>
    /// <param name="predicate">Предикат поиска</param>
    /// <param name="includeProperties">Включаемые параметры</param>
    /// <returns>Коллекция объектов по переданному фильтру</returns>
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