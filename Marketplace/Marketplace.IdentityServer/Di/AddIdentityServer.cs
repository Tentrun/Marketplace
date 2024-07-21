using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Identity.Di;

public static class AddIdentityServer
{
    public static void AddIdentitySettings(this IServiceCollection services)
    {
        services.ConfigureIdentityRequirements();
        services.AddIdentityInfrastructureSettings();
    }
    
    /// <summary>
    /// Конфигурация требований Identity
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureIdentityRequirements(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(opt =>
        {
            //Обязательное использование чисел в пароле
            opt.Password.RequireDigit = false;
            
            //Минимальная длина пароля
            opt.Password.RequiredLength = 6;
            
            //Обязательное использование спец символов
            opt.Password.RequireNonAlphanumeric = false;
            
            //Обязательное использование заглавного символа
            opt.Password.RequireUppercase = false;
            
            //Обязательное использование кол-ва спец символов
            opt.Password.RequiredUniqueChars = 0;
            
            //Уникальный адрес электронной почты для пользователя
            opt.User.RequireUniqueEmail = true;

            //Подтверждения, для использования сервисов
            opt.SignIn.RequireConfirmedAccount = false;
            opt.SignIn.RequireConfirmedEmail = false;
            opt.SignIn.RequireConfirmedPhoneNumber = false;
        });
    }

    private static void AddIdentityInfrastructureSettings(this IServiceCollection services)
    {
        services
            .AddIdentity<IdentityUserModel, IdentityRole<long>>()
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddUserManager<UserManager<IdentityUserModel>>()
            .AddRoleManager<RoleManager<IdentityRole<long>>>();
    }
}