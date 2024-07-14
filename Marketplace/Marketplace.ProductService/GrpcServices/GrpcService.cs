using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Utils.Base.Extension;
using Marketplace.BaseLibrary.Utils.Base.Logger;
using Marketplace.ProductService.Infrastructure.Interfaces;
using static Marketplace.BaseLibrary.Utils.Base.Extension.BaseRequestExtension;

namespace Marketplace.ProductService.GrpcServices;

public class GrpcService : BaseRequestService.BaseRequestServiceBase
{
    private readonly IProductsService _productsService;

    public GrpcService(IProductsService productsService)
    {
        _productsService = productsService;
    }

    public override async Task<BaseResponse> SendRequest(BaseRequest request, ServerCallContext context)
    {
        Logger.LogInformation($"Получен запрос {request.ToString()}");
        try
        {
            switch (string.IsNullOrWhiteSpace(request.TargetServiceMethod))
            {
                case false:
                    var result = await _productsService.CallService(request.TargetServiceMethod, request.RequestBody);
                    return GenerateBaseResponse(request, result);
                
                case true:
                    return request.GenerateBaseResponseWithError("Не указано имя метода для обращения");
            }
        }
        catch (Exception e)
        {
            return request.GenerateBaseResponseWithError(e.Message);
        }
    }
}