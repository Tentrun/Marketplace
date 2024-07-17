using Microsoft.Extensions.Hosting;

namespace Marketplace.BaseLibrary.Utils.Base.Settings.HealthCheckWorker;

/// <summary>
/// Добавляет в бэкграунд воркер для хелзчека сервиса
/// </summary>
/// <param name="name">Имя сервиса</param>
public class HealthCheckBackgroundWorker(string name, string? serviceDescription = "") : BackgroundService
{
    /// <summary>
    /// Таймаут при вылете исключения, по стандарту 15 секунд
    /// </summary>
    private TimeSpan ErrorTimeout { get; } = TimeSpan.FromSeconds(15);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var nextRun = DateTime.Now;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var timeout = Math.Max(0, (int)nextRun.Subtract(DateTime.Now).TotalMilliseconds);
            await Task.Delay(timeout, stoppingToken).ConfigureAwait(false);
            
            try
            {
                await HealthCheckService.SendHealthReport(name, serviceDescription).ConfigureAwait(false);

                nextRun = DateTime.Now.AddSeconds(60);
            }
            catch (System.Exception ex)
            {
                //TODO: приделать логгер, если отвалился SettingsService
                await Task.Delay(ErrorTimeout, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}