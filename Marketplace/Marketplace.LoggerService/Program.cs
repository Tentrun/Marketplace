using Marketplace.LoggerService.Data;
using Marketplace.LoggerService.Data.Repository.Implementation;
using Marketplace.LoggerService.Data.Repository.Interface;
using Marketplace.LoggerService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("LoggerPsSql"));
});
builder.Services.AddScoped<ILogRepository, LogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<LoggerService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();