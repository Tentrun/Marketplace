using System.Text;
using Google.Protobuf.WellKnownTypes;
using Marketplace.BaseLibrary.Const;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using static Marketplace.BaseLibrary.Utils.Base.Logger.Logger;

namespace Marketplace.BaseLibrary.Utils.Base.Extension;

public static class BaseRequestExtension
{
    /// <summary>
    /// Проверяет базовый запрос на валидность
    /// </summary>
    /// <param name="request">Базовый запрос</param>
    /// <param name="error">Текст ошибки при валидации</param>
    /// <returns>Результат валидации</returns>
    public static bool ValidateBaseRequest(this BaseRequest request, out string? error)
    {
        error = null;
        var errors = new List<string>();
        
        if (!Guid.TryParse(request.Id, out Guid id))
        {
            errors.Add("Не передан ID запроса в базовом запросе шлюза");
        }

        if (request.Date.ToDateTime() == DateTime.UnixEpoch || request.Date.ToDateTime() == DateTime.MinValue)
        {
            errors.Add($"Не передана дата запроса в базовом запросе шлюза, ID запроса {id}");
        }

        if (string.IsNullOrWhiteSpace(request.CallerServiceName))
        {
            errors.Add($"Не передано имя сервиса, откуда был вызван сервис в базовом запросе шлюза, ID запроса {id}");
        }
        
        if (string.IsNullOrWhiteSpace(request.TargetServiceName))
        {
            errors.Add($"Не передано имя сервиса, куда идет вызов в базовом запросе шлюза, ID запроса {id}");
        }
        
        if (string.IsNullOrWhiteSpace(request.TargetServiceMethod))
        {
            errors.Add($"Не передано имя метода, куда идет вызов в базовом запросе шлюза, ID запроса {id}");
        }

        //Если размер листа ошибок 0 - валидация пройдена, ошибок нету, запрос валиден
        if (errors.Count <= 0) 
            return true;
        
        //Создаем текст ошибки из листа
        
        //Что бы не отжирать память конкетинацией строк - создаем стринг билдер
        StringBuilder errorBuilder = new StringBuilder();
            
        //Проходимся по массиву
        errors.ForEach(error => errorBuilder.Append($"{error}\n"));

        //Текст ошибки создан - логируем
        error = errorBuilder.ToString();
        LogCritical(error);
        return false;
    }
    
    /// <summary>
    /// Создает базовый запрос с заполнеными полями и пустым телом запроса
    /// </summary>
    /// <param name="targetService">Сервис для обращения</param>
    /// <param name="targetServiceMethod">Метод сервиса для обращения</param>
    /// <param name="callerMethod"></param>
    /// <returns></returns>
    public static BaseRequest GenerateServiceBaseRequest(string targetService, string targetServiceMethod, string callerService)
    {
        if (string.IsNullOrWhiteSpace(targetService) || string.IsNullOrWhiteSpace(targetServiceMethod) || string.IsNullOrWhiteSpace(callerService))
        {
            throw new InvalidOperationException("Не переданы обязательные параметры для создания запроса");
        }
        
        var request = new BaseRequest
        {
            Id = Guid.NewGuid().ToString(),
            Date = DateTime.UtcNow.ToTimestamp(),
            TargetServiceName = targetService,
            TargetServiceMethod = targetServiceMethod,
            CallerServiceName = callerService,
            RequestBody = new Struct()
        };

        return request;
    }

    public static async Task<BaseResponse> SendRequestToGateway(BaseRequest request)
    {
        var gatewaySetting = await SettingsBaseService.GetGrpcServiceChannelByName(ServicesConst.ApiGateway);
        var client = new BaseRequestService.BaseRequestServiceClient(gatewaySetting);
        return await client.SendRequestAsync(request);
    }
}