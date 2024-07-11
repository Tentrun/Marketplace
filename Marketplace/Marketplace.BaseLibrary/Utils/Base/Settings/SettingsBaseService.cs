using Grpc.Net.Client;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Dto;
using Marketplace.BaseLibrary.Enum.Base;

namespace Marketplace.BaseLibrary.Utils.Base.Settings;

public static class SettingsBaseService
{
    public static async Task<GrpcChannel> GetGrpcServiceChannelByName(string serviceName)
    {
        var serviceSettings = await SettingsDbHandler.GetServiceSetting(serviceName);
        
        return GrpcChannel.ForAddress($"http://{serviceSettings.Ip}:{serviceSettings.Port}", new GrpcChannelOptions
        {
            MaxRetryAttempts = 5,
            MaxReconnectBackoff = TimeSpan.FromSeconds(5),
            InitialReconnectBackoff = TimeSpan.FromSeconds(3)
        });
    }

    /// <summary>
    /// Возвращает все инстансы для вебки
    /// TODO: это стоит куда-то убрать, когда добавим балансер, уберем из базовой реализации, ибо кроме вебки, это никому не нужно
    /// </summary>
    public static async Task<List<ServiceInstanceDTO>> GetAllInstances()
    {
        try
        {
            var settingsService = new SettingsGrpcService.SettingsGrpcServiceClient(await GetGrpcServiceChannelByName(ServicesConst.SettingsService));
            var response = await settingsService.GetInstancesStatusesAsync(new GetInstancesStatusesRequest());
            var result = new List<ServiceInstanceDTO>();
            foreach (var service in response.ServiceInstances)
            {
                result.Add(new ServiceInstanceDTO(service.Ip, service.Port, (ServiceStatusEnum)service.ServiceStatus, service.ServiceName, service.Description));
            }
            return result;
        }
        catch (System.Exception e)
        {
            Logger.Logger.LogCritical(e);
            return new List<ServiceInstanceDTO>();
        }
    }
}