using System.Text;
using Marketplace.BaseLibrary.Utils.Base.Logger;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.JwtExtension;

/// <summary>
/// Конфигурация настроек генерации токенов
/// </summary>
public static class TokensConfig
{
    /// <summary>
    /// Время жизни рефреш токена в днях (если не задано, стандартное - 14 дней)
    /// </summary>
    public static double RefreshTokenExpiryTime { get; private set; } = 14;
    
    /// <summary>
    /// Время жизни токена доступа в минутах (если не задано, стандартное - 60 минут)
    /// </summary>
    public static double TokenLifetime { get; private set; } = 60;

    /// <summary>
    /// Издатель токена
    /// </summary>
    public static string TokenIssuer { get; private set; } = string.Empty;
    
    /// <summary>
    /// Потребитель токена
    /// </summary>
    public static string TokenAudience { get; private set; } = string.Empty;

    /// <summary>
    /// Секретное слово для генерации токена доступа
    /// </summary>
    public static string TokenSecret { get; private set; } = "87bcCc02j2cB5n42!!9b$_3@c0b5E0@)e12d7Au8";

    /// <summary>
    /// Шифрование токена по указанному алгоритму и кодовому слову (стандарт Sha256)
    /// </summary>
    public static SigningCredentials SignToken()
    {
        return new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenSecret)), SecurityAlgorithms.HmacSha256);
    }
    
    public static void Initialize(IConfiguration configuration)
    {
        var identitySection = configuration.GetSection("Identity");
        
        if (!identitySection.Exists())
        {
            Logger.LogWarning("Секция identity не указана, для генерации токенов доступа будет использованы стандартные параметры! Обязательно внесите необходимые значения в секцию identity!");
        }
        
        if (double.TryParse(identitySection["RefreshTokenLifetime"], out double refreshTokenExpiryTime))
        {
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
        
        if (double.TryParse(identitySection["TokenLifetime"], out double tokenLifetime))
        {
            TokenLifetime = tokenLifetime;
        }

        TokenIssuer = identitySection["TokenIssuer"] ?? string.Empty;
        TokenAudience = identitySection["TokenAudience"] ?? string.Empty;
        TokenSecret = (string.IsNullOrWhiteSpace(identitySection["TokenSecret"]) ? "87bcCc02j2cB5n42!!9b$_3@c0b5E0@)e12d7Au8" : identitySection["TokenSecret"])!;
    }
}