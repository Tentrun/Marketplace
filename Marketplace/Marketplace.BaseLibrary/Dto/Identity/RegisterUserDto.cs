namespace Marketplace.BaseLibrary.Dto.Identity;

/// <summary>
/// Модель запроса на регистрацию
/// </summary>
/// <param name="Name">Имя пользователя</param>
/// <param name="SecondName">Фамилия пользователя</param>
/// <param name="Patronymic">Отчество пользователя</param>
/// <param name="Email">Адрес электронной почты пользователя</param>
/// <param name="Phone">Номер телефона пользователя</param>
/// <param name="Password">Пароль пользователя</param>
public record RequestRegisterUserDto(string Name, string SecondName, string? Patronymic, string Email, string? Phone, string Password);