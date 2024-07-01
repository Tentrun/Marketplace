using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;

namespace Marketplace.BaseLibrary.Utils.HealthCheckWorker;

public static class HealthCheckService
{
    public static string? ApplicationAddress = string.Empty;
    
    internal static async Task<bool> SendHealthReport(string serviceName, string? description = "")
    {
        if (string.IsNullOrWhiteSpace(ApplicationAddress))
        {
            //TODO: Если будем NLog подключать, логировать что хелзчеку сервиса нехорошо, потому что адрес инстанса не проброшен
            throw new NullReferenceException($"{nameof(ApplicationAddress)} передан пустой адрес инстанса");
        }

        var serviceAddress = ApplicationAddress.FormatAddress();
        var client = GetSettingsClient();
        var result = await client.SendHealthReportAsync(new HealthReportRequest
        {
            ServiceName = serviceName,
            Ip = serviceAddress.EndPoint.Host,
            Port = serviceAddress.EndPoint.Port,
            ServiceStatus = ServiceStatus.Available,
            Description = null
        });
        
        return result.Updated;
    }

    private static SettingsGrpcService.SettingsGrpcServiceClient GetSettingsClient()
    {
        GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5283");
        return new SettingsGrpcService.SettingsGrpcServiceClient(channel);
    }

    private static BalancerAddress FormatAddress(this string address)
    {
        var array = address.Split('/');
        var addresses = array[2].Split(':');
        if (addresses.Length > 3)
        {
            addresses[0] = "localhost";
            addresses[1] = addresses[3];
        }
        return new BalancerAddress(addresses[0],int.Parse(addresses[1]));
    }
}