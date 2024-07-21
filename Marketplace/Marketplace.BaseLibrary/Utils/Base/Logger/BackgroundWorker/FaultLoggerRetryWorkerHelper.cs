using Marketplace.BaseLibrary.Entity.Base.Logger;

namespace Marketplace.BaseLibrary.Utils.Base.Logger.BackgroundWorker;

public static class FaultLoggerRetryWorkerHelper
{
    /// <summary>
    /// Список логов, которые не смогли записаться с первого раза
    /// </summary>
    private static readonly List<Log> FaultLogsToWrite = new();

    /// <summary>
    /// Очищает список ошибочных логов
    /// </summary>
    internal static void ClearFaultLogs()
    {
        FaultLogsToWrite.Clear();
    }

    /// <summary>
    /// Возвращает список ошибочных логов
    /// </summary>
    /// <returns></returns>
    public static List<Log> GetFaultLogs()
    {
        return FaultLogsToWrite;
    }

    /// <summary>
    /// Добавляет в список оишбочный лог
    /// </summary>
    /// <param name="log"></param>
    public static void AddFaultLog(Log log)
    {
        FaultLogsToWrite.Add(log);
    }
}