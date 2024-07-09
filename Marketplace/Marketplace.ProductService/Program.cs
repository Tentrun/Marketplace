using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Di;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.Settings.HealthCheckWorker;
using Marketplace.BaseLibrary.Utils.Settings.HealthCheckWorker.DI;
using Marketplace.BaseLibrary.Utils.UnitOfWork.DI;
using Marketplace.ProductService.Data;
using Marketplace.ProductService.Data.Repository.Implementation;
using Marketplace.ProductService.Data.Repository.Interface;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("ProductsPsSql"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddUnitOfWork<ApplicationDbContext>();
builder.Services.AddDatabaseHealthReporter(ServicesConst.ProductService, "Сервис продуктов");
BaseInfrastructureDi.AddBaseServicesToDi(builder.Configuration);

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

//Применение авто миграций, если существуют новые добавленные
app.Services.ApplyMigrations<ApplicationDbContext>();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();