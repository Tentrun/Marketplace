using Marketplace.BaseLibrary.Dto.Identity;
using Marketplace.BaseLibrary.Entity.Identity;

namespace Marketplace.Identity.Services.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="requestRegisterUserDto">Запрос на регистрацию пользователя</param>
    public Task<bool> RegisterUser(RequestRegisterUserDto requestRegisterUserDto);

    /// <summary>
    /// Добавить пользователя к роли
    /// </summary>
    /// <param name="user">Сущность пользователя из БД</param>
    /// <param name="role">Имя роли</param>
    public Task<bool> AddUserToRole(IdentityUserModel? user, string role);

    /// <summary>
    /// Получает юзера по переданному номеру телефона или адресу электронной почты
    /// </summary>
    /// <param name="userData">Телефон или адрес электронной почты пользователя</param>
    public Task<IdentityUserModel?> GetUser(string? userData);
}