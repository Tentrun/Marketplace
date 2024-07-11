using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

SettingsDbHandler.ConfigureBaseSettingsService(builder.Configuration);
builder.Services.AddDatabaseHealthReporter(ServicesConst.SettingsService, "Сервис настроек");

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.MapGrpcService<ApiGatewayService>();
app.MapGet("/",
    () =>
        "ApiGateway is working!");

app.Run();