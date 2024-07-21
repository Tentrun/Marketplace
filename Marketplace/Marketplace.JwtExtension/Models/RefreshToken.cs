using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.JwtExtension.Models;

public class RefreshToken
{
    /// <summary>
    /// Ид рефреш токена
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Значение токена
    /// </summary>
    [Required]
    public string Token { get; set; }
    
    /// <summary>
    /// Дата истечения токена обновления
    /// </summary>
    [Required]
    public DateTime ExpiryOn { get; set; }
    
    /// <summary>
    /// Дата создания токена
    /// </summary>
    [Required]
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// Ид пользователя, которому принадлежит токен
    /// </summary>
    [Required]
    public long UserId { get; set; }

    /// <summary>
    /// Был ли отозван токен
    /// </summary>
    public bool IsRevoked { get; set; } = false;
}