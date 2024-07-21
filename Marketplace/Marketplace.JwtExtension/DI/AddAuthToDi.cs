using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.JwtExtension.DI;

public static class AddAuthToDi
{
    public static void AddIdentityToDi(this IServiceCollection services, IConfiguration configuration)
    {
        TokensConfig.Initialize(configuration);
        
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    //Ессли Issuer или Audience не заданы - не валидируем
                    ValidateIssuer = !string.IsNullOrEmpty(TokensConfig.TokenIssuer),
                    ValidateAudience = !string.IsNullOrEmpty(TokensConfig.TokenAudience),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new []{TokensConfig.TokenIssuer},
                    ValidAudience = TokensConfig.TokenAudience,
                    IssuerSigningKey = TokensConfig.SignToken().Key
                };
            });
        
        services.AddAuthorization(opt => opt.DefaultPolicy =
            new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
    }
}