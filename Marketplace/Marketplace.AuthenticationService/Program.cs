using Marketplace.AuthenticationService.Repository.Implementation;
using Marketplace.AuthenticationService.Repository.Interface;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.HealthCheckWorker.DI;
using Marketplace.BaseLibrary.Utils.UnitOfWork.DI;
using Marketplace.LoggerService.Data;
using Marketplace.LoggerService.Data.Repository.Implementation;
using Marketplace.LoggerService.Data.Repository.Interface;
using Marketplace.LoggerService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Marketplace.BaseLibrary.Utils.HealthCheckWorker.HealthCheckService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LoggerPsSql"));
});
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
builder.Services.AddUnitOfWork<ApplicationDbContext>();
builder.Services.AddDatabaseHealthReporter(ServicesConst.AuthenticationService, "Сервис аунтефикации");

var app = builder.Build();

//Применение авто миграций, если существуют новые добавленные
app.Services.ApplyMigrations<ApplicationDbContext>();

//Нужно для получения адреса приложения для хелзчекера из рантайма, но, выглядит стремно
app.Lifetime.ApplicationStarted.Register(() =>
{
    var server = app.Services.GetRequiredService<IServer>();
    var serverAddressesFeature = server.Features.Get<IServerAddressesFeature>();

    if (serverAddressesFeature == null) return;

    ApplicationAddress =
        serverAddressesFeature.Addresses.FirstOrDefault(x => x.Contains("http"));
});

// Configure the HTTP request pipeline.
app.MapGrpcService<LoggerService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();