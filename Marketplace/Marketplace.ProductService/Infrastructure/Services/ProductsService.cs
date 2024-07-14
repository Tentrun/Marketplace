using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Implementation;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Utils.Base.Extension;
using Marketplace.ProductService.Data.Repository.Interface;
using Marketplace.ProductService.Infrastructure.Interfaces;

namespace Marketplace.ProductService.Infrastructure.Services;

public class ProductsService : BaseService, IProductsService 
{
    public ProductsService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
    
    /// <summary>
    /// Получение продуктов дня
    /// </summary>
    public async Task GetProductsOfTheDay()
    {
        await using var productsRepository = UnitOfWork.GetRepository<IProductsRepository>();
        var products = await productsRepository.GetAllProductsOfTheDay();

        var response = new Struct();
        response.AddFieldToGrpcStruct("response", products);
        ServiceResponse = response;
    }
}