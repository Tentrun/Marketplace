using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.HealthCheckWorker;
using Marketplace.BaseLibrary.Utils.HealthCheckWorker.DI;
using Marketplace.BaseLibrary.Utils.UnitOfWork.DI;
using Marketplace.SettingsService.Data;
using Marketplace.SettingsService.Data.Repository.Implementation;
using Marketplace.SettingsService.Data.Repository.Interface;
using Marketplace.SettingsService.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("SettingsPsSql"));
});
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddUnitOfWork<ApplicationDbContext>();
builder.Services.AddDatabaseHealthReporter(ServicesConst.SettingsService, "Сервис настроек");

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