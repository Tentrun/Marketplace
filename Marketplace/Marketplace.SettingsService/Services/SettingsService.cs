using Grpc.Core;
using Marketplace.BaseLibrary;
using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Enum.Base;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;
using Marketplace.SettingsService.Data.Repository.Interface;

namespace Marketplace.SettingsService.Services;

public class SettingsService(IUnitOfWork unitOfWork) : SettingsGrpcService.SettingsGrpcServiceBase
{
    private readonly ISettingRepository _repository = unitOfWork.GetRepository<ISettingRepository>(); 
    
    /// <summary>
    /// Обновляет информацию в БД по информации из хелз репорта
    /// </summary>
    public override async Task<HealthReportResponse> SendHealthReport(HealthReportRequest request, ServerCallContext context)
    {
        return new HealthReportResponse
        {
            Updated = await _repository.UpdateServiceInfoFromHealthReport(new ServiceSetting
            {
                Ip = request.Ip,
                Port = request.Port,
                ServiceStatusEnum = (ServiceStatusEnum)request.ServiceStatus,
                Name = request.ServiceName,
                Description = request.Description,
                CreateDate = DateTime.UtcNow,
                UpdateDate = default
            })
        };
    }

    /// <summary>
    /// Получает настройки инстанса
    /// </summary>
    public override async Task<ServiceSettingsResponse> GetServiceSettings(ServiceSettingsRequest request,
        ServerCallContext context)
    {
        var setting = await _repository.GetServiceSettingByNameAsync(request.ServiceName);
        
        //Я внатуре сука по пальцам въебашу, если тут сука будет null из-за пустого имени, я же не шучу
        return new ServiceSettingsResponse
        {
            Ip = setting?.Ip,
            Port = setting?.Port
        };
    }

    /// <summary>
    /// Метод в принципе преднозначен для вебки, получает все инстансы из БД под вывод в вебке
    /// </summary>
    public override async Task<GetInstancesStatusesResponse> GetInstancesStatuses(GetInstancesStatusesRequest request,
        ServerCallContext context)
    {
        var instances = await _repository.GetAllInstances();
        var response = new GetInstancesStatusesResponse();
        foreach (var instance in instances)
        {
            response.ServiceInstances.Add(new ServiceInstance
            {
                ServiceName = instance.Name,
                Ip = instance.Ip,
                Port = instance.Port,
                ServiceStatus = (ServiceStatus)instance.ServiceStatusEnum,
                Description = instance.Description
            });
        }

        return response;
    }
}