using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationService.Common;

public class JwtToken
{
    public const string ISSUER = nameof(Services.AuthorizationService); // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public const int LIFETIME = 1; // время жизни токена - 1 минута
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }

    public static string Create(ClaimsIdentity claimsIdentity)
    {
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: ISSUER,
            audience: AUDIENCE,
            notBefore: DateTime.UtcNow,
            claims: claimsIdentity.Claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(LIFETIME)),
            signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}