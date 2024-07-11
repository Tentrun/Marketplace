using Google.Protobuf.WellKnownTypes;

namespace Marketplace.ProductService.Infrastructure.Interfaces;

public interface IProductsService
{
    public Struct GetProductsOfTheDay();
}