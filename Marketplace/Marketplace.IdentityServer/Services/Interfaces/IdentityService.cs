using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.Identity.Services.Implementations;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Identity.Services.Interfaces;

public class IdentityService : IIdentityService
{
    private readonly IUserService _userService;
    private readonly UserManager<IdentityUserModel> _userManager;

    public IdentityService(UserManager<IdentityUserModel> userManager, IUserService userService)
    {
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<bool> AuthenticateUser(string login, string password)
    {
        //Если был передан пустой параметр - выходим
        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        var user = await _userService.GetUser(login);
        //Если пользователь не был найден - выходим
        if (user == null)
        {
            return false;
        }

        //Возвращаем результат валидации пароля
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public Task<bool> AuthorizeUser()
    {
        throw new NotImplementedException();
    }
}