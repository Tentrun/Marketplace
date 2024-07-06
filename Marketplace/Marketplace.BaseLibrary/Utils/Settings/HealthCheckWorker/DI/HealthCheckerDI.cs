using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils.Settings.HealthCheckWorker.DI;

public static class HealthCheckerDi
{
    public static void AddDatabaseHealthReporter(this IServiceCollection services, string serviceName,
        string? serviceDescription = "")
    {
        services.AddHostedService(_ =>
        {
            var dispatcher = new HealthCheckBackgroundWorker(serviceName, serviceDescription);
            return dispatcher;
        });
    }
}