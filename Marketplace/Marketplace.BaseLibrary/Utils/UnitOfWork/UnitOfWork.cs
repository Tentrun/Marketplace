using System.Collections;
using Marketplace.BaseLibrary.Exception.Data;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Marketplace.BaseLibrary.Utils.UnitOfWork;

public class UnitOfWork<C> : IUnitOfWork where C : DbContext
{
    public UnitOfWork(C dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _repositories = new Hashtable();
    }

    private readonly DbContext _dbContext;
    private readonly Hashtable _repositories;

    private bool _disposed;

    public T GetRepository<T>() where T : class
    {
        string type = typeof(T).Name;
        if (_repositories.ContainsKey(type))
        {
            T? _repo = (T)_repositories[type];
            if (_repo is null)
            {
                throw new CannotAccessToRepository($"Репозиторий {nameof(T)} is null");
            }
            return (T)_repositories[type];
        }

        T? repo = _dbContext.GetService<T>();
        _repositories.Add(type, repo);

        if (repo is null)
        {
            throw new CannotAccessToRepository($"Не удалось получить репозиторий {nameof(T)}");
        }
        
        return repo;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}