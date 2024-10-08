using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Di;
using Marketplace.BaseLibrary.Utils;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;
using Marketplace.Identity.Data;
using Marketplace.Identity.Data.Repositories.Implementations;
using Marketplace.Identity.Data.Repositories.Interfaces;
using Marketplace.Identity.Di;
using Marketplace.Identity.Services.Implementations;
using Marketplace.Identity.Services.Interfaces;
using Marketplace.JwtExtension.DI;
using IdentityGrpcService = Marketplace.Identity.GrpcServices.IdentityGrpcService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

//Добавляем Identity в DI
builder.Services.AddIdentitySettings();

//Добавляем настройки для валидации JWT
builder.Services.AddIdentityToDi(builder.Configuration);

builder.Services.AddBaseServicesToDi<ApplicationIdentityDbContext>(builder.Configuration,"IdentityDb");
builder.Services.AddDatabaseHealthReporter(ServicesConst.Identity, "Сервис аутентификации и авторизации");

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddControllers();;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
app.MapGrpcService<IdentityGrpcService>();
//Применение авто миграций, если существуют новые добавленные
app.Services.ApplyMigrations<ApplicationIdentityDbContext>();

app.UseAuthorization();
app.MapGet("/",
    () =>
        $"{ServicesConst.Identity} work");

app.Run();