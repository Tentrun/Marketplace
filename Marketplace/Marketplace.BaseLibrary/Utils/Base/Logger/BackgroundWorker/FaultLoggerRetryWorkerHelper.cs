using Marketplace.BaseLibrary.Entity.Base.Logger;

namespace Marketplace.BaseLibrary.Utils.Base.Logger.BackgroundWorker;

public static class FaultLoggerRetryWorkerHelper
{
    private static readonly List<Log> FaultLogsToWrite = new();

    internal static void ClearFaultLogs()
    {
        FaultLogsToWrite.Clear();
    }

    public static List<Log> GetFaultLogs()
    {
        return FaultLogsToWrite;
    }

    public static void AddFaultLog(Log log)
    {
        FaultLogsToWrite.Add(log);
    }
}