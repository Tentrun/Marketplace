using Marketplace.BaseLibrary.Entity.Logger;
using Marketplace.LoggerService.Data.Repository.Interface;

namespace Marketplace.LoggerService.Data.Repository.Implementation;

public class LogRepository(IServiceScopeFactory serviceScopeFactory, ILogger<LogRepository> logger) : ILogRepository
{
    /// <summary>
    /// Функция записи лога
    /// </summary>
    /// <exception cref="NullReferenceException">В скопе нету контекста БД</exception>
    public async Task<bool> WriteLog(Log log)
    {
        try
        {
            //Получаем контекст из скопа
            using var scope = serviceScopeFactory.CreateScope();
            await using ApplicationDbContext? applicationDbContext = scope?.ServiceProvider?.GetService<ApplicationDbContext>();

            if (applicationDbContext == null)
            {
                throw new NullReferenceException($"В скопе отсутствует {nameof(applicationDbContext)}");
            }

            await applicationDbContext.ServiceLogs.AddAsync(log);

            return true;
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}