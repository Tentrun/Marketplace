namespace Marketplace.BaseLibrary.Enum.Base;

/// <summary>
/// Enum статусов доступности сервиса
/// </summary>
public enum ServiceStatusEnum
{
    /// <summary>
    /// Доступен
    /// </summary>
    Available = 0,
    
    /// <summary>
    /// Нагружен
    /// </summary>
    Busy = 1,
    
    /// <summary>
    /// Дамы и господа, инстансу пиздец
    /// </summary>
    Offline = 2
}