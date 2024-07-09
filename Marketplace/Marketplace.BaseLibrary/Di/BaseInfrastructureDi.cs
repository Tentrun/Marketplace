using Marketplace.BaseLibrary.Utils.Settings;
using Microsoft.Extensions.Configuration;

namespace Marketplace.BaseLibrary.Di;

/// <summary>
/// Конфигурирует подключение и настройки базовой инфраструктуры
/// </summary>
public static class BaseInfrastructureDi
{
    public static void AddBaseServicesToDi(IConfiguration configuration)
    {
        SettingsDbHandler.ConfigureBaseSettingsService(configuration);
    }
}