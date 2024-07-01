using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.SettingsService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// ДбСет сервисов
    /// </summary>
    public DbSet<ServiceSetting> ServiceSettings { get; set; }
}