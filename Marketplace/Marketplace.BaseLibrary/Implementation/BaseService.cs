using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.BaseLibrary.Interfaces.Base.Service;
using Marketplace.BaseLibrary.Utils.Base.Logger;

namespace Marketplace.BaseLibrary.Implementation;

public abstract class BaseService : IBaseService
{
    protected readonly IUnitOfWork UnitOfWork;
    
    /// <summary>
    /// Тело ответа сервиса
    /// </summary>
    protected Struct? ServiceResponse = null;
    
    /// <summary>
    /// Тело запроса
    /// </summary>
    private Struct _requestBody = null!;

    protected BaseService(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }

    /// <summary>
    /// Базовая операция вызова метода сервиса для получения ответа
    /// </summary>
    /// <param name="method">Имя вызываемого метода из запроса</param>
    /// <param name="requestBody">Тело запроса</param>
    /// <returns>Тело ответа от сервиса</returns>
    /// <exception cref="NullReferenceException">Сервис выполнил задачу, однако вернул null</exception>
    public async Task<Struct> CallService(string method, Struct requestBody)
    {
        //Записываем тело запроса, для его последующей доступности из сервиса наследника
        _requestBody = requestBody;
        
        //Получаем метод от текущего сервиса
        MethodInfo? requestedMethod = GetType().GetMethod(method);

        //Преобразуем в таску для ожидания окончания операции
        Task task = (Task)requestedMethod!.Invoke(this, null)!;
        
        //Запускам таску и ожидаем ее завершение в том же потоке
        await Task.Run(() => task.ConfigureAwait(false));

        //Дожидаемся выполнение запущенной ранее таски
        Task.WaitAny(task);
        
        //Проверяем не вернул ли сервис ответ null
        if (ServiceResponse == null) 
        {
            Logger.LogCritical($"Метод {method} вернул в ответ null");
            throw new NullReferenceException($"Метод {method} вернул в ответ null");
        }

        //Возвращаем ответ тому, кто вызывал сервис
        return ServiceResponse;
    }
}