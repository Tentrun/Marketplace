using Marketplace.BaseLibrary.Entity.Identity;

namespace Marketplace.Identity.Data.Repositories.Implementations;

public interface IIdentityRepository
{
    /// <summary>
    /// Получает пользователя по привязанному номеру телефона
    /// </summary>
    /// <param name="phone">Номер телефона пользователя</param>
    public Task<IdentityUserModel?> GetUserByPhone(string phone);
    
    /// <summary>
    /// Получается пользователя по привязанному адресу электронной почты
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    public Task<IdentityUserModel?> GetUserByEmail(string email);

    public Task<bool> AddUserToRole(IdentityUserModel user, string role);
}