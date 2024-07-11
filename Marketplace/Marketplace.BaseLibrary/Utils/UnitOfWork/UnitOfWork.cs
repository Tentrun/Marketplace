using System.Collections;
using Marketplace.BaseLibrary.Exception.Data;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils.UnitOfWork;

public class UnitOfWork<C> : IUnitOfWork where C : DbContext
{
    private readonly Hashtable _repositories;
    private readonly IServiceProvider _serviceProvider;

    private bool _disposed;
    
    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _repositories = new Hashtable();
    }

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

        //Т.к. я переделал на фабрику контекстов, но, есть желание сохранить unitOfWork
        //Придется делать скоп, оттуда доставать сервайс
        //Если использовать _content.GetService<T> упадет, т.к. контекста теперь нету в скопе, он без инжекта
        var repo = _serviceProvider.GetRequiredService<T>();
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
                //_dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}