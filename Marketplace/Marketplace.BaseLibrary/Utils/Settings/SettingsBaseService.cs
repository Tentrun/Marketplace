using Grpc.Net.Client;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Dto;
using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Exception.Settings;

namespace Marketplace.BaseLibrary.Utils.Settings;

public static class SettingsBaseService
{
    private static async Task<ServiceSetting> GetServiceSettingByName(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new SettingsServiceIsEmptyException("Передан пустой идентификатор сервиса для получения настроек");
        }

        var settingsService = new SettingsGrpcService.SettingsGrpcServiceClient(await GetGrpcServiceChannelByName(ServicesConst.SettingsService));
        var serviceSettings = await settingsService.GetServiceSettingsAsync(new ServiceSettingsRequest
        {
            ServiceName = serviceName
        });

        if (string.IsNullOrWhiteSpace(serviceSettings.Ip) || serviceSettings.Port == null)
        {
            throw new SettingsServiceIsEmptyException($"Для сервиса {serviceName} были получены пустые настройки!");
        }

        return new ServiceSetting
        {
            Ip = serviceSettings.Ip,
            Port = serviceSettings.Port.Value,
        };
    }

    public static async Task<GrpcChannel> GetGrpcServiceChannelByName(string serviceName)
    {
        //TODO: запихнуть сюда как-то динамически адреса для настроек

        if (serviceName == ServicesConst.SettingsService)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5283");
            return channel;
        }

        var serviceSettings = await GetServiceSettingByName(serviceName);
        
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