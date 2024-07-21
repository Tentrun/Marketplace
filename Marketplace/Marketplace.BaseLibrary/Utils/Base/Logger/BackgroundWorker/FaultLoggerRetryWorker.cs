using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Utils.Base.Logger.Extension;
using Microsoft.Extensions.Hosting;

namespace Marketplace.BaseLibrary.Utils.Base.Logger.BackgroundWorker;

public class FaultLoggerRetryWorker : BackgroundService
{
    private TimeSpan ErrorTimeout { get; } = TimeSpan.FromSeconds(120);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var nextRun = DateTime.Now;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var timeout = Math.Max(0, (int)nextRun.Subtract(DateTime.Now).TotalMilliseconds);
            await Task.Delay(timeout, stoppingToken).ConfigureAwait(false);
            
            try
            {
                var faultLogs = FaultLoggerRetryWorkerHelper.GetFaultLogs();
                
                if (faultLogs.Count > 0)
                {
                    var loggerClient = await LoggerExtension.GetLoggerClient();

                    foreach (var log in faultLogs)
                    {
                        var response = await loggerClient.CreateLogAsync(new LogRequest
                        {
                            LogType = (LogType)log.LogType,
                            LogDate = log.Time.ToTimestamp(),
                            LogClassInitializer = log.CallingClass,
                            LogCallingMethod = log.CallingMethod,
                            LogJsonValue = log.LogValue
                        });
                        
                        //Если не смогли записать заново, прекращаем цикл
                        if (!response.WriteLogResult)
                        {
                            break;
                        }
                    }
                    
                    FaultLoggerRetryWorkerHelper.ClearFaultLogs();
                }

                nextRun = DateTime.Now.AddSeconds(120);
            }
            catch (System.Exception ex)
            {
                await Task.Delay(ErrorTimeout, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}