using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;

namespace Marketplace.SettingsService.Data.Repository.Interface;

public interface IServiceRepository
{
    /// <summary>
    /// Возвращает коллекцию сервисов, которые не проходят по таймауту оффлайна
    /// </summary>
    internal Task<List<ServiceSetting>> GetUnavailableServices();
    
    /// <summary>
    /// Изменяет статус на offline для инстансов, у которых updateTime превысил время таймаута
    /// </summary>
    /// <param name="services">Сервисы с превышенным таймаутом</param>
    internal Task<bool> ChangeStatusForTimeoutServices(List<ServiceSetting> services);
}