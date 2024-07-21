using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.JwtExtension.Models;

namespace Marketplace.Identity.Data.Repositories.Implementations;

public interface IIdentityRepository
{
    /// <summary>
    /// Получает пользователя по привязанному номеру телефона
    /// </summary>
    /// <param name="phone">Номер телефона пользователя</param>
    public Task<IdentityUserModel?> GetUserByPhone(string phone);

    /// <summary>
    /// Получает пользователя по его ИД
    /// </summary>
    /// <param name="id">ИД пользователя</param>
    public Task<IdentityUserModel?> GetUserById(long id);
    
    /// <summary>
    /// Получается пользователя по привязанному адресу электронной почты
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    public Task<IdentityUserModel?> GetUserByEmail(string email);

    /// <summary>
    /// Добавляет юзера к указанной роли
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="role">Имя роли</param>
    public Task<bool> AddUserToRole(IdentityUserModel user, string role);

    /// <summary>
    /// Добавляет новый рефреш токен в базу данных
    /// </summary>
    /// <param name="refreshToken"></param>
    public Task<bool> AddRefreshToken(RefreshToken refreshToken);

    /// <summary>
    /// Получает сущность токена из БД по его значению
    /// </summary>
    /// <param name="tokenValue">Значение токена</param>
    public Task<RefreshToken?> GetRefreshToken(string tokenValue);
    
    /// <summary>
    /// Обновляет рефреш токен после его использования
    /// </summary>
    /// <param name="refreshToken"></param>
    public Task<bool> UpdateRefreshToken(RefreshToken refreshToken);

    /// <summary>
    /// Отзыв рефреш токена по его ID
    /// </summary>
    /// <param name="tokenId">Ид рефреш токена</param>
    public Task<bool> RevokeRefreshToken(Guid tokenId);
}