using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Enum.Base;

namespace Marketplace.BaseLibrary.Entity.Base.User;

/// <summary>
/// Сушность пользователя для записи в БД
/// </summary>
public class User
{
    public User(string name,
        string patronimic,
        string surname,
        DateTime createdTime,
        DateTime? updatedTime,
        string phone,
        bool phoneConfirmed,
        string email,
        bool emailConfirmed,
        UserRoleEnum userRole,
        string userAlias,
        string password,
        string passwordHash)
    {
        Name = name;
        Patronimic = patronimic;
        Surname = surname;
        CreatedTime = createdTime;
        UpdatedTime = updatedTime;
        Phone = phone;
        PhoneConfirmed = phoneConfirmed;
        Email = email;
        EmailConfirmed = emailConfirmed;
        UserRole = userRole;
        UserAlias = userAlias;
        Password = password;
        PasswordHash = passwordHash;
    }

    public User(UserRequest request)
    {
        Name = request.User.Name;
        Patronimic = request.User.Patronimic;
        Surname = request.User.Surname;
        CreatedTime = request.User.CreatedTime.ToDateTime();
        UpdatedTime = request.User.UpdatedTime.ToDateTime();
        Phone = request.User.Phone;
        PhoneConfirmed = request.User.PhoneConfirmed;
        Email = request.User.Email;
        EmailConfirmed = request.User.EmailConfirmed;
        UserRole = (UserRoleEnum)request.User.UserRole;
        UserAlias = request.User.UserAlias;
        Password = request.User.Password;
        PasswordHash = request.User.PasswordHash;
    }
    
    /// <summary>
    /// Авто генерируемйы ключ в БД
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    /// <summary>
    /// Имя
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Отчество
    /// </summary>
    public string Patronimic { get; private set; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    public string Surname { get; private set; }
    
    /// <summary>
    /// Ник пользователя
    /// </summary>
    public string UserAlias { get; private set; }
    
    /// <summary>
    /// Время создания учётной записи
    /// </summary>
    public DateTime CreatedTime { get; private set; }
    
    /// <summary>
    /// Время редактирования учётной записи
    /// </summary>
    public DateTime? UpdatedTime { get; private set; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string Phone { get; private set; }    
    
    /// <summary>
    /// Потдверждён ли телефон
    /// </summary>
    public bool PhoneConfirmed { get; private set; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; private set; }
    
    /// <summary>
    /// Потдверждёна ли почта
    /// </summary>
    public bool EmailConfirmed { get; private set; }
    
    /// <summary>
    /// Тип пользователя
    /// </summary>
    public UserRoleEnum UserRole { get; private set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; private set; }
    
    /// <summary>
    /// Hash пароля
    /// </summary>
    public string PasswordHash { get; private set; }
}