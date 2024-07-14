using System.Collections;
using Marketplace.BaseLibrary.Exception.Data;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork
{
    private static readonly Hashtable Repositories = new();
    private readonly IServiceProvider _serviceProvider;
    
    private bool _disposed;
    
    public UnitOfWork(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public T GetRepository<T>() where T : class
    {
        string type = typeof(T).Name;
        if (Repositories.ContainsKey(type))
        {
            T? _repo = (T)Repositories[type];
            if (_repo is null)
            {
                throw new CannotAccessToRepository($"Репозиторий {nameof(T)} is null");
            }
            return (T)Repositories[type];
        }

        //Т.к. я переделал на фабрику контекстов, но, есть желание сохранить unitOfWork
        //Придется делать скоп, оттуда доставать сервайс
        //Если использовать _content.GetService<T> упадет, т.к. контекста теперь нету в скопе, он без инжекта
        var repo = _serviceProvider.GetRequiredService<T>();
        Repositories.Add(type, repo);

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

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}