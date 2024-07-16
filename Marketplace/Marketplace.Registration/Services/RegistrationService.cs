using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.Registration.Repository.Interface;
using Microsoft.Extensions.Logging;
using User = Marketplace.BaseLibrary.Entity.Base.User.User;

namespace Marketplace.Registration.Services;

public class RegistrationService(IUnitOfWork unitOfWork, ILogger<RegistrationService> logger) : RegistrationGrpcService.RegistrationGrpcServiceBase
{
    public override async Task<UserReply> CreateUser(UserRequest request, ServerCallContext context)
    {
        try
        {
            var registrationRepository = unitOfWork.GetRepository<IRegistrationRepository>();
            
            return new UserReply
            {
                WriteLogResult = await registrationRepository.CreateAsync(new User(request))
            };
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}