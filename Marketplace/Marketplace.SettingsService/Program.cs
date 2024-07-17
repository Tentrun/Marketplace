using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Di;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;
using Marketplace.SettingsService.BackgroundWorker;
using Marketplace.SettingsService.Data;
using Marketplace.SettingsService.Data.Repository.Implementation;
using Marketplace.SettingsService.Data.Repository.Interface;
using Marketplace.SettingsService.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddBaseServicesToDi<ApplicationDbContext>(builder.Configuration, "SettingsPsSql");
builder.Services.AddDatabaseHealthReporter(ServicesConst.SettingsService, "Сервис настроек");
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

//Бекграунд воркер обновления статусов инстансов
builder.Services.AddHostedService(x =>
{
    var scope = x.CreateScope();
    var requiredService = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var dispatcher = new ServicesStatusUpdaterBackgroundWorker(requiredService);
    return dispatcher;
});

var app = builder.Build();

//Применение авто миграций, если существуют новые добавленные
app.Services.ApplyMigrations<ApplicationDbContext>();

//Нужно для получения адреса приложения для хелзчекера из рантайма, но, выглядит стремно
app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<IServer>();
    var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

    if (serverAddressesFeature == null) return;

    HealthCheckService.ApplicationAddress =
        serverAddressesFeature.Addresses.FirstOrDefault(x => x.Contains("http"));
});

// Configure the HTTP request pipeline.
app.MapGrpcService<SettingsService>();
app.MapGet("/",
    () =>
        $"{ServicesConst.SettingsService} work");

app.Run();