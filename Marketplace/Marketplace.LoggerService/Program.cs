using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Di;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;
using Marketplace.LoggerService.Data;
using Marketplace.LoggerService.Data.Repository.Implementation;
using Marketplace.LoggerService.Data.Repository.Interface;
using Marketplace.LoggerService.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddBaseServicesToDi<ApplicationDbContext>(builder.Configuration, "LoggerPsSql");
builder.Services.AddDatabaseHealthReporter(ServicesConst.LoggerService, "Сервис логгера");
builder.Services.AddScoped<ILogRepository, LogRepository>();

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
app.MapGrpcService<LoggerService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();