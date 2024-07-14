using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using static Marketplace.BaseLibrary.Utils.Base.Logger.Logger;

namespace Marketplace.BaseLibrary.Utils.Base.Extension;

public static class BaseGrpcStructExtension
{
    /// <summary>
    /// Добавляет объект в связку ключ-значние в тело struct
    /// </summary>
    /// <param name="body">Тело запроса</param>
    /// <param name="paramName">Имя параметра (ключ)</param>
    /// <param name="obj">Значение параметра (значение)</param>
    public static void AddFieldToGrpcStruct(this Struct? body, string paramName, object? obj)
    {
        //Проверяем не переданы ли пустые параметры, если ключ или значние пустое - выходим
        if (obj == null || string.IsNullOrWhiteSpace(paramName))
        {
            return;
        }

        if (body == null)
        {
            body = new Struct();
        }
        
        //Сериализованный объект, который будет являться значнием поля
        string? serializedObject;

        //Пробуем сериализовать объект
        try
        {
            serializedObject = JsonSerializer.Serialize(obj);
        }
        
        //В случае ошибки - выходим
        catch (System.Exception e)
        {
            LogCritical($"Ошибка сериализации значения для параметра - {paramName} \n {e}");
            return;
        }

        //Если каким-то образом сериализованный объект оказался пустым - выходим
        if (string.IsNullOrWhiteSpace(serializedObject))
        {
            LogWarning($"Попытка добавления пустого значения в параметр {paramName}");
            return;
        }
        
        //Добавляем пару ключ-значние в тело запроса
        body.Fields.Add(paramName, new Value{StringValue = serializedObject});
    }
}