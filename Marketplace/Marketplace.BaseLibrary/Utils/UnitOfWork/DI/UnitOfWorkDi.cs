using Marketplace.BaseLibrary.Enum;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils.UnitOfWork.DI;

/// <summary>
/// Класс для подключения в DI контейнер UnitOfWork
/// </summary>
public static class UnitOfWorkDi
{
    /// <summary>
    /// Добавляет UnitOfWork в DI контейнер
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <param name="lifetime">Время жизни зависимости</param>
    /// <returns>Модифицированная коллекция сервисов</returns>
    public static IServiceCollection AddUnitOfWork<T>(this IServiceCollection services, DiLifetimeEnum lifetime = DiLifetimeEnum.Scoped) where T :  DbContext
    {
        switch (lifetime)
        {
            case DiLifetimeEnum.Transient:
                services.AddTransient<IUnitOfWork, UnitOfWork<T>>();
                break;
            case DiLifetimeEnum.Scoped:
                services.AddScoped<IUnitOfWork, UnitOfWork<T>>();
                break;
            case DiLifetimeEnum.Singleton:
                services.AddSingleton<IUnitOfWork, UnitOfWork<T>>();
                break;
        }

        return services;
    }
}