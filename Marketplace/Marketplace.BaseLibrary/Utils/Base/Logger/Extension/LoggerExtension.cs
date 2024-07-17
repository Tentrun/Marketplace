using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using static System.String;

namespace Marketplace.BaseLibrary.Utils.Base.Logger.Extension;

internal static class LoggerExtension
{
    /// <summary>
    /// Убирает путь до файла, оставляя название класса
    /// </summary>
    internal static string FormatCallingMethod(this string? value)
    {
        return IsNullOrEmpty(value) ? Empty : Path.GetFileNameWithoutExtension(value);
    }

    /// <summary>
    /// Возвращает проинициализированного gRpc клиента логгера
    /// TODO: переделать на фабрику, а то все упадет в дальнейшем
    /// </summary>
    internal static async Task<LoggerGrpcService.LoggerGrpcServiceClient> GetLoggerClient()
    {
        return new LoggerGrpcService.LoggerGrpcServiceClient(await SettingsBaseService.GetGrpcServiceChannelByName(ServicesConst.LoggerService));
    }
}