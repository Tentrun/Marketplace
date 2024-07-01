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
}