using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Marketplace.BaseLibrary.Enum.Base;

namespace Marketplace.BaseLibrary.Entity.Base.ServiceSettings;

/// <summary>
/// Модель настройки сервиса
/// </summary>
public class ServiceSetting
{
    /// <summary>
    /// Уникальный ключ в БД инстанса
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    
    /// <summary>
    /// Адрес сервиса
    /// </summary>
    [Required]
    public string Ip { get; init; }
    
    /// <summary>
    /// Порт сервиса
    /// </summary>
    [Required]
    public int Port { get; init; }
    
    /// <summary>
    /// Статус доступности сервиса
    /// </summary>
    [Required]
    public ServiceStatusEnum ServiceStatusEnum { get; set; }
    
    /// <summary>
    /// Название инстанса (сервиса)
    /// </summary>
    [Required]
    public string Name { get; init; }
    
    /// <summary>
    /// Описание сервиса
    /// А нужно ли оно?
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Дата и время создания инстанса
    /// </summary>
    public DateTime CreateDate { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// Время последнего обновления доступности сервиса
    /// </summary>
    public DateTime UpdateDate { get; set; }
}