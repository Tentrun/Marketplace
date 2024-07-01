using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Interfaces.Base;
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
        //Если сюда кто-то передал пустое имя, получит по пальцам клавиатурой, а потом топором
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return null;
        }
        return await context.ServiceSettings.FirstOrDefaultAsync(x => x.Name == serviceName);;
    }

    public async Task<bool> UpdateServiceInfoFromHealthReport(ServiceSetting serviceSetting)
    {
        try
        {
            var currentServiceSetting = await GetServiceSettingByNameAsync(serviceSetting.Name);
            if (currentServiceSetting is null)
            {
                serviceSetting.UpdateDate = DateTime.UtcNow;
                await CreateAsync(serviceSetting);
                return true;
            }

            currentServiceSetting.UpdateDate = DateTime.UtcNow;
            await UpdateAsync(currentServiceSetting);
            return true;
        }
        catch (Exception e)
        {
            //TODO: Приделать логгер при ошибке
            Console.WriteLine(e);
            throw;
        }
    }
}