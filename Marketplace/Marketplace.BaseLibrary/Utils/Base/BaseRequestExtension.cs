using Google.Protobuf.WellKnownTypes;

namespace Marketplace.BaseLibrary.Utils.Base;

public static class BaseRequestExtension
{
    /// <summary>
    /// Проверяет базовый запрос на валидность
    /// </summary>
    /// <param name="request">Базовый запрос</param>
    /// <param name="error">Текст ошибки при валидации</param>
    /// <returns>Результат операции</returns>
    public static bool ValidateBaseRequest(this BaseRequest request, out string? error)
    {
        error = null;
        
        if (!Guid.TryParse(request.Id, out Guid id))
        {
            error = $"Не передан ID запроса в базовом запросе шлюза";
        }

        if (request.Date.ToDateTime() == DateTime.UnixEpoch || request.Date.ToDateTime() == DateTime.MinValue)
        {
            error = $"Не передана дата запроса в базовом запросе шлюза, ID запроса {id}";
        }

        if (string.IsNullOrWhiteSpace(request.CallerServiceName))
        {
            error = $"Не передано имя сервиса, откуда был вызван сервис в базовом запросе шлюза, ID запроса {id}";
        }
        
        if (string.IsNullOrWhiteSpace(request.TargetServiceName))
        {
            error = $"Не передано имя сервиса, куда идет вызов в базовом запросе шлюза, ID запроса {id}";
        }
        
        if (string.IsNullOrWhiteSpace(request.TargetServiceMethod))
        {
            error = $"Не передано имя метода, куда идет вызов в базовом запросе шлюза, ID запроса {id}";
        }

        if (error != null)
        {
            Logger.Logger.LogCritical(error);
            return false;
        }
        
        return true;
    }

    public static BaseResponse GenerateBaseResponseWithError(this BaseRequest request, string errorText, string? errorCode = null)
    {
        var response = new BaseResponse
        {
            Id = request.Id ?? string.Empty,
            Date = request.Date ?? DateTime.UtcNow.ToTimestamp(),
            ErrorBody = GenerateBaseResponseError(errorText, errorCode)
        };
        return response;
    }
    
    /// <summary>
    /// Генерация структуры ошибки для базового ответа
    /// </summary>
    /// <param name="errorText">Текст ошибки</param>
    /// <param name="errorCode">Код ошибки</param>
    /// <returns>Struct value с кодом и текстом ошибки</returns>
    private static Struct GenerateBaseResponseError(string errorText, string? errorCode = null)
    {
        var response = new Struct();
        
        //Если не передали код ошибки, по дефолту будет 500
        if (string.IsNullOrWhiteSpace(errorCode))
        {
            errorCode = "500";
        }
        
        response.Fields.Add("errorCode", new Value{ StringValue = errorCode });
        response.Fields.Add("errorText", new Value{ StringValue = errorText });

        return response;
    }

    /// <summary>
    /// Конструктор базового ответа
    /// </summary>
    /// <param name="request">Базовый запрос к сервису</param>
    /// <param name="responseBody">Ответ от сервиса</param>
    public static BaseResponse GenerateBaseResponse(BaseRequest request, Struct? responseBody)
    {
        var response = new BaseResponse
        {
            Id = request.Id,
            Date = DateTime.UtcNow.ToTimestamp(),
            ResponseBody = null
        };

        if (responseBody != null)
        {
            response.ResponseBody = responseBody;
            return response;
        }
        
        throw new NullReferenceException(
            "В конструктор базового ответа не был передан ответ, формирование ответа невозможно!");
    }
}