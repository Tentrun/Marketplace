using Marketplace.BaseLibrary.Entity.Products;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ProductService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Перевод на legacy систему времени постгре
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    /// <summary>
    /// ДбСет продуктов
    /// </summary>
    public DbSet<Product> Products { get; set; }
}