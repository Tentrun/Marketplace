using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.JwtExtension.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.Identity.Data;

public class ApplicationIdentityDbContext : IdentityDbContext<IdentityUserModel, IdentityRole<long>, long>
{
    private readonly IConfiguration _configuration;
    
    /// <summary>
    /// ДбСет рефреш токенов
    /// </summary>
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("IdentityDb"));
        options.EnableThreadSafetyChecks();
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        //По стандарту они занеймлены криво, ренеймим
        builder.Entity<IdentityUserModel>().ToTable("Users", "public");
        builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens", "public");
        builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles", "public");
        builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims", "public");
        builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims", "public");
        builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins", "public");
        builder.Entity<IdentityRole<long>>().ToTable("Roles", "public");

        builder.Entity<IdentityRole<long>>().HasData(new List<IdentityRole<long>>
        {
            new()
            {
                Id = 1,
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new()
            {
                Id = 2,
                Name = "Seller",
                NormalizedName = "SELLER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            },
            new()
            {
                Id = 3,
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        });
    }
}