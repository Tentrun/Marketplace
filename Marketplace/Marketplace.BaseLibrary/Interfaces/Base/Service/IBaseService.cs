using Google.Protobuf.WellKnownTypes;

namespace Marketplace.BaseLibrary.Interfaces.Base.Service;

/// <summary>
/// Базовый сервис
/// </summary>
public interface IBaseService : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Базовая операция вызова метода сервиса для получения ответа
    /// </summary>
    /// <param name="method">Имя вызываемого метода из запроса</param>
    /// <param name="requestBody">Тело запроса</param>
    /// <returns>Тело ответа от сервиса</returns>
    /// <exception cref="NullReferenceException">Сервис выполнил задачу, однако вернул null</exception>
    public Task<Struct> CallService(string method, Struct requestBody);
}