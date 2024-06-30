namespace Marketplace.BaseLibrary.Interfaces.Base.Repository;

public interface IUnitOfWork : IDisposable
{
    T GetRepository<T>() where T: class;
}