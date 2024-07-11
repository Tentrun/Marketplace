using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marketplace.BaseLibrary.Utils.Base.Settings;

/// <summary>
/// Контекст необходимый для решения проблемы "0 сервиса"
/// </summary>
public class SettingsBaseDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public SettingsBaseDbContext(DbContextOptions<SettingsBaseDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("SettingsDb"));
        options.EnableThreadSafetyChecks();
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// ДбСет сервисов
    /// </summary>
    public DbSet<ServiceSetting> ServiceSettings { get; set; }
}