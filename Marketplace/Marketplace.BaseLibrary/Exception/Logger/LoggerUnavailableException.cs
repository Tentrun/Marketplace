namespace Marketplace.BaseLibrary.Exception.Logger;

/// <summary>
/// Использовать только для логгера при недоступности сервиса логгирования
/// </summary>
public class LoggerUnavailableException(string message) : InvalidOperationException(message);