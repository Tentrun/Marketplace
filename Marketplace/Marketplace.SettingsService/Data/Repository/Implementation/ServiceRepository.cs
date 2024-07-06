using EFCore.BulkExtensions;
using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Utils.Logger;
using Marketplace.SettingsService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.SettingsService.Data.Repository.Implementation;

internal class ServiceRepository(ApplicationDbContext context) : IServiceRepository
{
    /// <summary>
    /// Возвращает коллекцию сервисов, которые не проходят по таймауту оффлайна
    /// </summary>
    public async Task<List<ServiceSetting>> GetUnavailableServices()
    {
        try
        {
            DateTime currentTime = DateTime.UtcNow;
            var result = await context.ServiceSettings
                //Так же легче высчитать на серваке по дате, ибо выборка по времени очень грузит БД
                .Where(x => x.ServiceStatusEnum != ServiceStatusEnum.Offline)
                .ToListAsync();
            return result.Where(x => x.UpdateDate.AddMinutes(3) < currentTime).ToList();
        }
        catch (Exception e)
        {
            Logger.LogCritical(e.ToString());
            return new List<ServiceSetting>();
        }
    }

    public async Task<bool> ChangeStatusForTimeoutServices(List<ServiceSetting> services)
    {
        if (services.Count == 0)
        {
            Logger.LogCritical("Произведена попытка обновить статусы инстансов, когда размер коллекции 0");
            return true;
        }
        services.ForEach(x => x.ServiceStatusEnum = ServiceStatusEnum.Offline);

        try
        {
            await context.BulkUpdateAsync(services);
            return true;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e.ToString());
            return false;
        }
    }
}