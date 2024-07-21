namespace Marketplace.BaseLibrary.Dto.Identity;

/// <summary>
/// Дто для передачи токенов
/// </summary>
/// <param name="accessToken">Токен доступа к приложению</param>
/// <param name="refreshToken">Токен обновления</param>
public struct TokensDto(string accessToken, string refreshToken);