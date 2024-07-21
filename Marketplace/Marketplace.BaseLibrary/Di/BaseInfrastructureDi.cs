using Marketplace.BaseLibrary.Utils.Base.Logger.BackgroundWorker;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using Marketplace.BaseLibrary.Utils.UnitOfWork.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Di;

/// <summary>
/// Конфигурирует подключение и настройки базовой инфраструктуры
/// </summary>
public static class BaseInfrastructureDi
{
    public static void AddBaseServicesToDi<T>(this IServiceCollection services, IConfiguration configuration, string connectionStringName) where T : DbContext
    {
        SettingsDbHandler.ConfigureBaseSettingsService(configuration);
        services.AddDbContextFactory<T>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString(connectionStringName));
            opt.EnableThreadSafetyChecks();
        });
        services.AddUnitOfWork();
        services.AddHostedService<FaultLoggerRetryWorker>();
    }
}