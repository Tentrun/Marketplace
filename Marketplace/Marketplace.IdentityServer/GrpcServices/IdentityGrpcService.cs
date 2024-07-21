using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Dto.Identity;
using Marketplace.BaseLibrary.Utils.Base.Logger;
using Marketplace.Identity.Services.Interfaces;

namespace Marketplace.Identity.GrpcServices;

public class IdentityGrpcService : BaseLibrary.IdentityGrpcService.IdentityGrpcServiceBase
{
    private readonly IUserService _userService;
    private readonly IIdentityService _identityService;

    public IdentityGrpcService(IUserService userService, IIdentityService identityService)
    {
        _userService = userService;
        _identityService = identityService;
    }

    public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
    {
        try
        {
            var result = await _identityService.RefreshTokens(request.RefreshToken);
            return new RefreshTokenResponse
            {
                RefreshToken = result.RefreshToken,
                AccessToken = result.AccessToken,
            };
        }
        catch (Exception e)
        {
            return new RefreshTokenResponse
            {
                Error = e.Message
            };
        }
    }

    public override async Task<RefreshTokenResponse> RegisterUser(RegisterRequest request, ServerCallContext context)
    {
        try
        {
            await _userService.RegisterUser(new RequestRegisterUserDto(request.Name, request.SecondName,
                request.Patronymic, request.Email, request.Phone, request.Password));

            var user = await _userService.GetUser(request.Email);
            var tokens = await _identityService.AuthorizeUser(user);

            return new RefreshTokenResponse
            {
                RefreshToken = tokens.RefreshToken,
                AccessToken = tokens.AccessToken,
            };
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return new RefreshTokenResponse
            {
                Error = e.Message
            };
        }
    }

    public override async Task<RefreshTokenResponse> AuthUser(AuthRequest request, ServerCallContext context)
    {
        try
        {
            var user = await _userService.GetUser(request.Login);
            var result = await _identityService.AuthenticateUser(user, request.Password);
            if (!result)
            {
                return new RefreshTokenResponse
                {
                    Error = "Неверный логин или пароль"
                };
            }

            var tokens = await _identityService.AuthorizeUser(user);
            return new RefreshTokenResponse
            {
                RefreshToken = tokens.RefreshToken,
                AccessToken = tokens.AccessToken
            };
        }
        catch (Exception e)
        {
            Logger.LogCritical(e);
            return new RefreshTokenResponse
            {
                Error = e.Message
            };
        }
    }
}