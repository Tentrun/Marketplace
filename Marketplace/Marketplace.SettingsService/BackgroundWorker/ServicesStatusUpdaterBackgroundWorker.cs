using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Utils.Base.Logger;
using Marketplace.SettingsService.Data.Repository.Interface;

namespace Marketplace.SettingsService.BackgroundWorker;

public class ServicesStatusUpdaterBackgroundWorker(IUnitOfWork unitOfWork) : BackgroundService
{
    /// <summary>
    /// Таймаут при вылете исключения, по стандарту 30 секунд
    /// </summary>
    private TimeSpan ErrorTimeout { get; } = TimeSpan.FromSeconds(15);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var nextRun = DateTime.Now;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var timeout = Math.Max(0, (int)nextRun.Subtract(DateTime.Now).TotalMilliseconds);
            await Task.Delay(timeout, stoppingToken).ConfigureAwait(false);
            
            try
            {
                var repository = unitOfWork.GetRepository<IServiceRepository>();

                var unavailableServices = await repository.GetUnavailableServices().ConfigureAwait(false);
                if (unavailableServices.Count > 0)
                {
                    if (!await repository.ChangeStatusForTimeoutServices(unavailableServices).ConfigureAwait(false))
                    {
                        Logger.LogCritical("Не удалось обновить статусы доступности инстансов по неизвестной причине!"); 
                    }
                }

                nextRun = DateTime.Now.AddMinutes(2);
            }
            catch (System.Exception ex)
            {
                Logger.LogCritical(ex); 
                await Task.Delay(ErrorTimeout, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}