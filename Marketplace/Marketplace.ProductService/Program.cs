using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.UnitOfWork.DI;
using Marketplace.ProductService.Data;
using Marketplace.ProductService.Data.Repository.Implementation;
using Marketplace.ProductService.Data.Repository.Interface;
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

var app = builder.Build();

//Применение авто миграций, если существуют новые добавленные
app.Services.ApplyMigrations<ApplicationDbContext>();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();