namespace Marketplace.JwtExtension.Models;

public class TokensPack
{
    /// <summary>
    /// Токен доступа к приложению
    /// </summary>
    public required string AccessToken { get; init; }
    
    /// <summary>
    /// Рефреш токен
    /// </summary>
    public required string RefreshToken { get; init; }
}