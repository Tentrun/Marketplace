using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.ProductService.Data.Repository.Implementation;
using Marketplace.ProductService.Infrastructure.Interfaces;

namespace Marketplace.ProductService.Infrastructure.Services;

public class ProductsService(IUnitOfWork unitOfWork) : IProductsService
{
    /// <summary>
    /// Получение продуктов дня
    /// </summary>
    public Struct GetProductsOfTheDay()
    {
        var repository = unitOfWork.GetRepository<ProductsRepository>();

        return new Struct();
    }
}