using Marketplace.BaseLibrary.Entity.Logger;

namespace Marketplace.LoggerService.Data.Repository.Interface;

public interface ILogRepository
{
    /// <summary>
    /// Функция записи лога
    /// </summary>
    /// <exception cref="NullReferenceException">В скопе нету контекста БД</exception>
    public Task<bool> WriteLog(Log log);
}