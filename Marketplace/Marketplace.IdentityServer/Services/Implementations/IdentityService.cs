using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.Identity.Data.Repositories.Implementations;
using Marketplace.Identity.Services.Interfaces;
using Marketplace.JwtExtension;
using Marketplace.JwtExtension.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Marketplace.Identity.Services.Implementations;

public class IdentityService : IIdentityService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUserModel> _userManager;

    public IdentityService(UserManager<IdentityUserModel> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AuthenticateUser(IdentityUserModel? user, string password)
    {
        //Если был передан пустой параметр - выходим
        if (string.IsNullOrWhiteSpace(password) || user == null)
        {
            return false;
        }
        
        //Возвращаем результат валидации пароля
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<TokensPack> AuthorizeUser(IdentityUserModel? user)
    {
        if (user == null)
        {
            throw new NullReferenceException("Пользователь не обнаружен в базе данных");
        }

        var userRoles = await GetUserRoles(user);

        if (userRoles == null)
        {
            throw new NullReferenceException("У пользователя отсутствуют роли");
        }
        
        var claims = new Dictionary<string, object>();
        userRoles.ForEach(x => claims.Add(ClaimTypes.Role, x));
        claims.Add("UserId", user.Id);

        var refreshToken = GenerateRefreshToken(user.Id);

        IIdentityRepository repository = _unitOfWork.GetRepository<IIdentityRepository>();
        await repository.AddRefreshToken(refreshToken);

        return new TokensPack
        {
            AccessToken = GenerateAccessToken(claims),
            RefreshToken = refreshToken.Token
        };
    }

    private string GenerateAccessToken(Dictionary<string, object> claims)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        DateTime currentDateTime = DateTime.UtcNow;
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = TokensConfig.TokenAudience,
            Expires = DateTime.UtcNow.AddMinutes(TokensConfig.TokenLifetime),
            Issuer = TokensConfig.TokenIssuer,
            IssuedAt = currentDateTime,
            NotBefore = currentDateTime,
            Claims = claims,
            SigningCredentials = TokensConfig.SignToken()
        };

        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);
        string accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }
    
    /// <summary>
    /// Генерация рефреш токена
    /// </summary>
    /// <param name="userId">Уникальный идентифкатор пользователя в таблице базы данных</param>
    /// <returns>Сущность рефреш токена</returns>
    private RefreshToken GenerateRefreshToken(long userId)
    {
        //Генерируем случайные байты для рефреш токена
        using RandomNumberGenerator randomBytesGenerator = RandomNumberGenerator.Create();
        byte[] randomBytes = new byte[64];
        randomBytesGenerator.GetBytes(randomBytes);
        
        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiryOn = DateTime.UtcNow.AddDays(TokensConfig.RefreshTokenExpiryTime),
            CreatedOn = DateTime.UtcNow,
            UserId = userId
        };
    }
    
    public async Task<List<string>?> GetUserRoles(IdentityUserModel? user)
    {
        if (user == null)
        {
            return null;
        }
        
        //Получаем роли юзера
        var roles = await _userManager.GetRolesAsync(user);
        return roles.Count > 0 ? roles.ToList() : null;
    }
}