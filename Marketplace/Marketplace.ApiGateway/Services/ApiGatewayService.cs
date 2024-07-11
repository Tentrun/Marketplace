using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Utils.Base;
using Marketplace.BaseLibrary.Utils.Base.Settings;

namespace Marketplace.ApiGateway.Services;

public class ApiGatewayService : BaseRequestService.BaseRequestServiceBase
{
    public override async Task<BaseResponse> SendRequest(BaseRequest request, ServerCallContext context)
    {
        if (!request.ValidateBaseRequest(out var error))
        {
            return request.GenerateBaseResponseWithError(error!);
        }

        var service = await SettingsBaseService.GetGrpcServiceChannelByName(request.TargetServiceName);
        var client = new BaseRequestService.BaseRequestServiceClient(service);
        var response = await client.SendRequestAsync(request);

        return response;
    }
}
