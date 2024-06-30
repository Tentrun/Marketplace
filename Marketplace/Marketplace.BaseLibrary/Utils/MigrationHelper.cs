using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils;

public static class MigrationHelper
{
    public static void ApplyMigrations<T>(this IServiceProvider provider) where T : DbContext
    {
        using var scope = provider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<T>();

        //Создает БД, если ее нету
        context.Database.EnsureCreated();
        
        //Проверка на новые миграции и последующее применение
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}