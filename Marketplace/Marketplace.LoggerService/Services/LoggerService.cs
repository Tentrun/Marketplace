using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Entity.Base.Logger;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.LoggerService.Data.Repository.Interface;

namespace Marketplace.LoggerService.Services;

public class LoggerService(IUnitOfWork unitOfWork, ILogger<LoggerService> logger) : LoggerGrpcService.LoggerGrpcServiceBase
{
    public override async Task<LogReply> CreateLog(LogRequest request, ServerCallContext context)
    {
        try
        {
            var logRepository = unitOfWork.GetRepository<ILogRepository>();
            
            return new LogReply
            {
                WriteLogResult = await logRepository.CreateAsync(new Log(request))
            };
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}