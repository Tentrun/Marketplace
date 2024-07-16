using System.Security.Claims;
using AuthorizationService.Common;
using AuthorizationService.Services.Interface;
using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Entity.Base.User;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Utils;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Services;

public class AuthorizationService(IUnitOfWork unitOfWork, ILogger<AuthorizationService> logger) : AuthorizationGrpcService.AuthorizationGrpcServiceBase
{
    public override async Task<AuthorizeReply> Authorize(AuthorizeRequest request, ServerCallContext context)
    {
        try
        {
            UserGrpc userGrpcr = await GetUser(request, context);
            return new AuthorizeReply
            {
                User = userGrpcr,
                Jwt = JwtToken.Create(
                    new ClaimsIdentity(
                        new List<Claim>
                        {
                            new(ClaimsIdentity.DefaultNameClaimType, userGrpcr.UserAlias),
                            new(ClaimsIdentity.DefaultRoleClaimType, userGrpcr.UserRole.ToString())
                        }, 
                        "Token", 
                        ClaimsIdentity.DefaultNameClaimType, 
                        ClaimsIdentity.DefaultRoleClaimType
                    )
                )
            };
        }
        catch (Exception e)
        {
            logger.LogCritical(e.ToString());
            throw;
        }
    }

    public override async Task<UserGrpc> GetUser(AuthorizeRequest request, ServerCallContext context)
    {
        try
        {
            var authorizationRepository = unitOfWork.GetRepository<IAuthorizationRepository>();

            User? user = await authorizationRepository.GetAsync(request);
            
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