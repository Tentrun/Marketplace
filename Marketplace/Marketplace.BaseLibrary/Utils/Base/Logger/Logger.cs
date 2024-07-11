using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Entity.Base.Logger;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Exception.Logger;
using Marketplace.BaseLibrary.Utils.Base.Logger.Extension;

namespace Marketplace.BaseLibrary.Utils.Base.Logger;

/// <summary>
/// Универсальный класс для записи логов в базу данных
/// </summary>
public static class Logger
{
    /// <summary>
    /// Логирование данных с явно указанными параметрами
    /// </summary>
    public static async void Log(LogTypeEnum logType, object logValue, [CallerMemberName] string caller = "", [CallerFilePath] string callerFilePath = "")
    {
        await ExecuteCreateLogOperation(
            new Log(logType, callerFilePath, caller,
                logValue)
        );
    }

    /// <summary>
    /// Логирование данных с типом информации
    /// </summary>
    public static async void LogInformation(object logValue ,[CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "")
    {
        await ExecuteCreateLogOperation(
            new Log(LogTypeEnum.Information, callerFilePath, caller,
                logValue)
        );
    }
    
    /// <summary>
    /// Логирование данных с типом предупреждения
    /// </summary>
    public static async void LogWarning(object logValue ,[CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "")
    {
        await ExecuteCreateLogOperation(
            new Log(LogTypeEnum.Warning, callerFilePath, caller,
                logValue)
        );
    }
    
    /// <summary>
    /// Логирование данных с типом ошибки
    /// </summary>
    public static async void LogCritical(object logValue ,[CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "")
    {
        await ExecuteCreateLogOperation(
            new Log(LogTypeEnum.Error, callerFilePath, caller,
                logValue)
        );
    }
    
    /// <summary>
    /// Логирование данных с типом отладки
    /// </summary>
    public static async void LogDebug(object logValue ,[CallerMemberName] string caller = "",
        [CallerFilePath] string callerFilePath = "")
    {
        await ExecuteCreateLogOperation(
            new Log(LogTypeEnum.Debug, callerFilePath, caller,
                logValue)
        );
    }
    
    /// <summary>
    /// Отправка запроса на запись лога сервису логирования
    /// </summary>
    private static async Task ExecuteCreateLogOperation(Log log)
    {
        try
        {
            var loggerClient = await LoggerExtension.GetLoggerClient();
            
            LogReply? reply = await loggerClient.CreateLogAsync(new LogRequest
            {
                LogType = (LogType)log.LogType,
                LogDate = log.Time.ToTimestamp(),
                LogClassInitializer = log.CallingClass,
                LogCallingMethod = log.CallingMethod,
                LogJsonValue = log.LogValue
            });
        }
        catch (System.Exception e)
        {
            throw new LoggerUnavailableException(e.ToString());
        }
    }
}