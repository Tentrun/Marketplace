using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Marketplace.BaseLibrary.Entity.Identity;

/// <summary>
/// Модель пользователя
/// </summary>

public class IdentityUserModel : IdentityUser<long>
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    [MaxLength(40)]
    public string SecondName { get; set; } = null!;

    /// <summary>
    /// Отчество
    /// </summary>
    /// <returns></returns>
    [MaxLength(40)]
    public string? Patronymic { get; set; }
    
    /// <summary>
    /// Дата регистрации
    /// </summary>
    public DateTime RegistrationDate { get; init; }
}