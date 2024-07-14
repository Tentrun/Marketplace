using Marketplace.BaseLibrary.Entity.Products;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;

namespace Marketplace.ProductService.Data.Repository.Interface;

public interface IProductsRepository : IBaseRepository<Product>
{
    /// <summary>
    /// Возвращает все продукты дня
    /// </summary>
    public Task<List<Product>> GetAllProductsOfTheDay();
}