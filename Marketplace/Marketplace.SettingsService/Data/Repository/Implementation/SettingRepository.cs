using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Interfaces.Base;
using Marketplace.BaseLibrary.Utils.Logger;
using Marketplace.SettingsService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.SettingsService.Data.Repository.Implementation;

public class SettingRepository(ApplicationDbContext context) : BaseRepository<ServiceSetting>(context), ISettingRepository
{
    /// <summary>
    /// Возвращает настройки сервиса по переданному названию сервиса (указывается в background worker'e)
    /// </summary>
    /// <param name="serviceName">Имя сервиса по которому необходимо получить настройки</param>
    /// <returns>Настройки сервиса</returns>
    public async Task<ServiceSetting?> GetServiceSettingByNameAsync(string serviceName)
    {
        try
        {
            //Если сюда кто-то передал пустое имя, получит по пальцам клавиатурой, а потом топором
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                return null;
            }

            var serviceSetting = await context.ServiceSettings.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Name == serviceName && x.ServiceStatusEnum != ServiceStatusEnum.Offline);

            //Проверяем не логгер ли был запрошен и вернулся null, если нет, то логгируем
            if (serviceName != ServicesConst.LoggerService && serviceSetting == null)
            {
                Logger.LogWarning($"Не обнаружено ни одного активного инстанса для сервиса {serviceName}");
                return null;
            }
            return serviceSetting;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return null;
        }
    }

    /// <summary>
    /// Обновляет информацию пришедшую по хелзчеку сервиса
    /// </summary>
    public async Task<bool> UpdateServiceInfoFromHealthReport(ServiceSetting serviceSetting)
    {
        try
        {
            var currentServiceSetting = await context.ServiceSettings.FirstOrDefaultAsync(x =>
                x.Name == serviceSetting.Name && x.Ip == serviceSetting.Ip && x.Port == serviceSetting.Port);
            
            if (currentServiceSetting is null)
            {
                serviceSetting.UpdateDate = DateTime.UtcNow;
                await CreateAsync(serviceSetting);
                return true;
            }

            currentServiceSetting.Description = serviceSetting.Description;
            currentServiceSetting.ServiceStatusEnum = serviceSetting.ServiceStatusEnum;
            currentServiceSetting.UpdateDate = DateTime.UtcNow;
            await UpdateAsync(currentServiceSetting);
            return true;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return false;
        }
    }

    public async Task<List<ServiceSetting>> GetAllInstances()
    {
        try
        {
            var result = await context.ServiceSettings.Where(x => x.Id != null)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return new List<ServiceSetting>();
        }
    }
}