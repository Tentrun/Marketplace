using Marketplace.BaseLibrary.Const.Identity;
using Marketplace.BaseLibrary.Dto.Identity;
using Marketplace.BaseLibrary.Entity.Identity;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.Identity.Data;
using Marketplace.Identity.Data.Repositories.Implementations;
using Marketplace.Identity.Services.Implementations;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.Identity.Services.Interfaces;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUserModel> _userManager;

    public UserService(UserManager<IdentityUserModel> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole<long>> roleManager, ApplicationIdentityDbContext context)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddUserToRole(IdentityUserModel? user, string role)
    {
        if (user == null)
        {
            return false;
        }
        
        //К сожалению штатный userManager не может нормально работать с трекующим контекстом и выдает ошибку, даже если отключить трек стейта
        var repository = _unitOfWork.GetRepository<IIdentityRepository>();
        await repository.AddUserToRole(user, role);

        return true;
    }

    public async Task<IdentityUserModel?> GetUser(string? userData)
    {
        //Если была передана пустая строка
        if (string.IsNullOrWhiteSpace(userData))
        {
            return null;
        }

        var repository = _unitOfWork.GetRepository<IIdentityRepository>();

        //Проверяем что было передано - телефон или эмейл
        bool isEmail = userData.Contains('@') && !userData.Contains('+');
        return isEmail switch
        {
            true => await repository.GetUserByEmail(userData),
            false => await repository.GetUserByPhone(userData)
        };
    }

    public async Task<bool> RegisterUser(RequestRegisterUserDto requestRegisterUserDto)
    {
        //Генерируем имя пользователя из крендиталсов
        string userName = string.IsNullOrWhiteSpace(requestRegisterUserDto.Patronymic)
            ? $"{requestRegisterUserDto.Name.ToUpperInvariant()}{requestRegisterUserDto.SecondName.ToUpperInvariant()}"
            : $"{requestRegisterUserDto.Name.ToUpperInvariant()}{requestRegisterUserDto.SecondName.ToUpperInvariant()}{requestRegisterUserDto.Patronymic.ToUpperInvariant()}";

        //Создаем модель пользователя из DTO
        var userModel = new IdentityUserModel
        {
            UserName = userName,
            NormalizedUserName = userName.ToLower(),
            Email = requestRegisterUserDto.Email.ToLower(),
            NormalizedEmail = requestRegisterUserDto.Email.ToLower(),
            PhoneNumber = requestRegisterUserDto.Phone,
            Name = requestRegisterUserDto.Name,
            SecondName = requestRegisterUserDto.SecondName,
            Patronymic = requestRegisterUserDto.Patronymic,
            RegistrationDate = DateTime.UtcNow
        };
        
        //Пытаемся создать пользователя
        var result = await _userManager.CreateAsync(userModel, requestRegisterUserDto.Password);

        if (!result.Succeeded) 
            return false;
        
        //Получаем юзера из БД
        IdentityUserModel? user = await GetUser(requestRegisterUserDto.Email) ?? await GetUser(requestRegisterUserDto.Phone);

        if (user == null)
        {
            return false;
        }

        //Добавляем его к стандартной роли
        await AddUserToRole(user, RolesConst.User);
            
        return true;
    }
}