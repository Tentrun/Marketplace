namespace Marketplace.Identity.Services.Implementations;

public interface IIdentityService
{
    /// <summary>
    /// Аутентификация пользователя по его данным
    /// </summary>
    /// <param name="login">Номер телефона или электронная почта пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    public Task<bool> AuthenticateUser(string login, string password);
    
    /// <summary>
    /// Авторизация по правам пользователя
    /// </summary>
    public Task<bool> AuthorizeUser();
}