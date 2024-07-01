using Microsoft.Extensions.DependencyInjection;

namespace Marketplace.BaseLibrary.Utils.HealthCheckWorker.DI;

public static class HealthCheckerDi
{
    public static IServiceCollection AddDatabaseHealthReporter(this IServiceCollection services, string serviceName, string? serviceDescription = "")
    {
        services.AddHostedService(_ =>
        {
            var dispatcher = new HealthCheckBackgroundWorker(serviceName, serviceDescription);
            return dispatcher;
        });

        return services;
    }
}