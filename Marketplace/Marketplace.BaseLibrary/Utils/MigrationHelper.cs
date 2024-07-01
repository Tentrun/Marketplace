using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils;

public static class MigrationHelper
{
    public static void ApplyMigrations<T>(this IServiceProvider provider) where T : DbContext
    {
        using var scope = provider.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<T>();

        switch (context.Database.CanConnect())
        {
            case true:
                //Проверка на новые миграции и последующее применение
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
                break;
            case false:
                //Создает БД, если ее нету
                //С миграцией, по доке миграцию в последующем не применить
                //Опасная штука
                //TODO: снести нахуй
                context.Database.EnsureCreated();
                break;
        }
    }
}