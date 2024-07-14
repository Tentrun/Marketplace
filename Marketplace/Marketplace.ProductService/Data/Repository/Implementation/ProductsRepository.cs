using Marketplace.BaseLibrary.Entity.Products;
using Marketplace.BaseLibrary.Implementation;
using Marketplace.ProductService.Data.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Marketplace.ProductService.Data.Repository.Implementation;

public class ProductsRepository : BaseRepository<Product, ApplicationDbContext>, IProductsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProductsRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }

    /// <summary>
    /// Возвращает все продукты дня
    /// </summary>
    public async Task<List<Product>> GetAllProductsOfTheDay()
    {
        var result = await _dbContext.ProductsOfTheDay.Where(x => x.Id != null).ToListAsync();
        await _dbContext.DisposeAsync();
        return result;
    }
}