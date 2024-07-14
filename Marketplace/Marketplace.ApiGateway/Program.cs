using Marketplace.ApiGateway.Services;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

SettingsDbHandler.ConfigureBaseSettingsService(builder.Configuration);
builder.Services.AddDatabaseHealthReporter(ServicesConst.ApiGateway, "Сервис гейтвей");

var app = builder.Build();

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
app.MapGrpcService<ApiGatewayService>();
app.MapGet("/",
    () =>
        "ApiGateway is working!");

app.Run();