using Marketplace.BaseLibrary.Entity.Logger;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.LoggerService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// ДбСет логов
    /// </summary>
    public DbSet<Log> ServiceLogs { get; set; }
}