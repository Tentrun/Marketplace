using Grpc.Net.Client;
using static System.String;

namespace Marketplace.BaseLibrary.Logger.Extension;

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
    internal static LoggerGrpcService.LoggerGrpcServiceClient LoggerClient()
    {
        GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5062");
        return new LoggerGrpcService.LoggerGrpcServiceClient(channel);
    }
}