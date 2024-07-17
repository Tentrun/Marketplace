using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Di;
using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker.DI;
using Marketplace.Identity.Data;
using Marketplace.Identity.Data.Repositories.Implementations;
using Marketplace.Identity.Data.Repositories.Interfaces;
using Marketplace.Identity.Services.Implementations;
using Marketplace.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBaseServicesToDi<ApplicationIdentityDbContext>(builder.Configuration,"IdentityDb");
builder.Services.AddDatabaseHealthReporter(ServicesConst.Identity, "Сервис аутентификации и авторизации");

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredUniqueChars = 1;
            
    opt.User.RequireUniqueEmail = true;

    opt.SignIn.RequireConfirmedAccount = false;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services
    .AddIdentity<IdentityUserModel, IdentityRole<long>>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddUserManager<UserManager<IdentityUserModel>>()
    .AddRoleManager<RoleManager<IdentityRole<long>>>();

builder.Services.AddControllers();;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();