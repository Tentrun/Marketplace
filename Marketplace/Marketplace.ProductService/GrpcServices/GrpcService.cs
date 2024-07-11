using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Utils.Base;
using Marketplace.BaseLibrary.Utils.Base.Logger;

namespace Marketplace.ProductService.GrpcServices;

public class GrpcService : BaseRequestService.BaseRequestServiceBase
{
    public override async Task<BaseResponse> SendRequest(BaseRequest request, ServerCallContext context)
    {
        Logger.LogInformation($"Получен запрос {request.ToString()}");
        var response = new BaseResponse();

        try
        {
            switch (request.TargetServiceMethod)
            {
                case "GetProductsOfTheDay":
                    //TODO: доделать
                    return new BaseResponse();
                    break;
                
                default:
                    return request.GenerateBaseResponseWithError("Не указано имя метода для обращения");
                    break;
            }
        }
        catch (Exception e)
        {
            return request.GenerateBaseResponseWithError(e.Message);
        }
    }
}