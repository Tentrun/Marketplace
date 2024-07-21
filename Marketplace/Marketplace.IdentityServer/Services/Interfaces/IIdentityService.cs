using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.JwtExtension.Models;

namespace Marketplace.Identity.Services.Interfaces;

public interface IIdentityService
{
    /// <summary>
    /// Аутентификация пользователя по его данным
    /// </summary>
    /// <param name="user">Пользователь для аутентификации</param>
    /// <param name="password">Пароль пользователя</param>
    public Task<bool> AuthenticateUser(IdentityUserModel? user, string password);
    
    /// <summary>
    /// Авторизация по правам пользователя
    /// </summary>
    public Task<TokensPack> AuthorizeUser(IdentityUserModel? user);

    /// <summary>
    /// Обновление связки токенов по рефреш токену
    /// </summary>
    /// <param name="refreshToken">Рефреш токен юзера</param>
    public Task<TokensPack> RefreshTokens(string refreshToken);
    
    /// <summary>
    /// Получение ролей пользователя
    /// </summary>
    /// <param name="user">Пользователь из базы данных</param>
    public Task<List<string>?> GetUserRoles(IdentityUserModel? user);
}