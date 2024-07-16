using Marketplace.BaseLibrary.Entity.Base.User;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.AuthenticationService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// ДбСет пользователей
    /// </summary>
    public DbSet<User> ServiceUsers { get; set; }
}