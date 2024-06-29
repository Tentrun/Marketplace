using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Entity.Logger;
using Marketplace.LoggerService.Data.Repository.Interface;

namespace Marketplace.LoggerService.Services;

public class LoggerService(ILogRepository logRepository, ILogger<LoggerService> logger) : LoggerGrpcService.LoggerGrpcServiceBase
{
    public override async Task<LogReply> CreateLog(LogRequest request, ServerCallContext context)
    {
        try
        {
            return new LogReply
            {
                WriteLogResult = await logRepository.WriteLog(new Log(request))
            };
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
        
    }
}