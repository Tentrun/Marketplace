using Google.Protobuf.WellKnownTypes;

namespace Marketplace.BaseLibrary.Utils.Base.Extension;

public static class BaseResponseExtension
{
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
}