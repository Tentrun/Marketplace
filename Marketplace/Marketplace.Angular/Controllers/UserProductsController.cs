using System.Text.Json;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Entity.Products;
using Marketplace.BaseLibrary.Utils.Base.Extension;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class UserProductsController : ControllerBase
{
    [HttpGet("GetProductsOfTheDay")]
    public async Task<List<Product>> GetProductsOfTheDay()
    {
        var request =
            BaseRequestExtension.GenerateServiceBaseRequest(ServicesConst.ProductService, "GetProductsOfTheDay",
                "Front");
        var response = await BaseRequestExtension.SendRequestToGateway(request);
        //TODO: сделать под json строку, не сериализуя повторно
        return JsonSerializer.Deserialize<List<Product>>(response.ResponseBody.Fields["response"].StringValue);
    }
}