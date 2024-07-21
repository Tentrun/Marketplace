using Marketplace.BaseLibrary.Di.Swagger;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using Marketplace.JwtExtension.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
SettingsDbHandler.ConfigureBaseSettingsService(builder.Configuration);

//Добавляем настройки для валидации JWT
builder.Services.AddIdentityToDi(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerToDi();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();