using Grpc.Core;
using Marketplace.AuthenticationService.Repository.Interface;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Entity.Base.User;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Utils;
using Microsoft.Extensions.Logging;

namespace Marketplace.AuthenticationService.Services;

public class AuthenticationService(IUnitOfWork unitOfWork, ILogger<AuthenticationService> logger) : AuthenticationGrpcService.AuthenticationGrpcServiceBase
{
    public override async Task<AuthenticationReply> Login(AuthenticationRequest request, ServerCallContext context)
    {
        try
        {
            return new AuthenticationReply
            {
                User = await GetUser(request, context)
            };
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }

    public override async Task<UserGrpc> GetUser(AuthenticationRequest request, ServerCallContext context)
    {
        try
        {
            var authenticationRepository = unitOfWork.GetRepository<IAuthenticationRepository>();

            User? user = await authenticationRepository.GetAsync(request);
            
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            return GrpcHelper.ConvertToGrpc(user);
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }
}