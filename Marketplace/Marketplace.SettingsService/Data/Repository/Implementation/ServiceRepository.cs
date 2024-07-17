using EFCore.BulkExtensions;
using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Utils.Base.Logger;
using Marketplace.SettingsService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.SettingsService.Data.Repository.Implementation;

internal class ServiceRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : IServiceRepository
{
    private readonly ApplicationDbContext _context = dbContextFactory.CreateDbContext();
    
    /// <summary>
    /// Возвращает коллекцию сервисов, которые не проходят по таймауту оффлайна
    /// </summary>
    public async Task<List<ServiceSetting>> GetUnavailableServices()
    {
        try
        {
            DateTime currentTime = DateTime.UtcNow;
            var result = await _context.ServiceSettings
                .Where(x => x.ServiceStatusEnum != ServiceStatusEnum.Offline && x.UpdateDate.AddMinutes(3) < currentTime)
                .ToListAsync();
            return result.ToList();
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
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
        
        services.ForEach(x =>
        {
            x.UpdateDate = DateTime.UtcNow;
            x.ServiceStatusEnum = ServiceStatusEnum.Offline;
        });

        try
        {
            await _context.BulkUpdateAsync(services);
            await _context.BulkSaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return false;
        }
    }
}