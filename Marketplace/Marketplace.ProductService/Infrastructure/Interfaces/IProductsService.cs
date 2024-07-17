using Marketplace.BaseLibrary.Interfaces.Base.Service;

namespace Marketplace.ProductService.Infrastructure.Interfaces;

public interface IProductsService : IBaseService
{
    public Task GetProductsOfTheDay();
}