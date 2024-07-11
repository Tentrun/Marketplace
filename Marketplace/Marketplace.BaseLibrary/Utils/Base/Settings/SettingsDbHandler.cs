using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Exception.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marketplace.BaseLibrary.Utils.Base.Settings;

public static class SettingsDbHandler
{
    /// <summary>
    /// appsettings
    /// </summary>
    private static IConfiguration? _configuration = null;

    /// <summary>
    /// Заносим в память конфигурацию билдера, для последующего использования connectionString
    /// </summary>
    public static void ConfigureBaseSettingsService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Возвращает настройки для подключения к сервису из БД
    /// </summary>
    /// <param name="service">Имя сервиса</param>
    /// <returns></returns>
    /// <exception cref="SettingsServiceIsEmptyException">Настройки сервиса null</exception>
    public static async Task<ServiceSetting> GetServiceSetting(string service)
    {
        var context = InitializeContext();
        //Сначала пытаемся отдать сервис с допустимой нагрузкой
        var serviceSetting = await context.ServiceSettings.FirstOrDefaultAsync(x => x.Name == service && x.ServiceStatusEnum == ServiceStatusEnum.Available);
        if (serviceSetting == null || string.IsNullOrEmpty(serviceSetting.Ip) || serviceSetting.Port <= 0)
        {
            //Пытаемся отдать загруженный сервис
            serviceSetting = await context.ServiceSettings.FirstOrDefaultAsync(x =>
                x.Name == service && x.ServiceStatusEnum == ServiceStatusEnum.Busy);
            
            //Если даже он null, или некорректные данные для подключения, отдаем ошибку
            //TODO: пока логгер является только сервисом, опасно пытаться логировать, ведь если это был он, будет прикол)
            if (serviceSetting == null || string.IsNullOrEmpty(serviceSetting.Ip) || serviceSetting.Port <= 0)
            {
                throw new SettingsServiceIsEmptyException($"Не обнаружено настройки для инстанса {service}");
            }
        }

        return serviceSetting;
    }
    
    /// <summary>
    /// Создание контекста
    /// Так исключаем шанс закрытия соединения, при долгой работе сервиса
    /// </summary>
    /// <returns>Контекст БД настроек</returns>
    /// <exception cref="SettingsServiceIsEmptyException">Не указаны данные для подключения в appSettings</exception>
    private static SettingsBaseDbContext InitializeContext()
    {
        var optionsBuilder = new DbContextOptions<SettingsBaseDbContext>();

        if (_configuration == null)
        {
            throw new SettingsServiceIsEmptyException("Для создания контекста БД не указаны параметры в appsettings");
        }
        return new SettingsBaseDbContext(optionsBuilder, _configuration);
    }
}