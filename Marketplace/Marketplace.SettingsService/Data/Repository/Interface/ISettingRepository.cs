using Marketplace.BaseLibrary.Entity.Base.ServiceSettings;
using Marketplace.BaseLibrary.Interfaces.Base.Repository;

namespace Marketplace.SettingsService.Data.Repository.Interface;

public interface ISettingRepository : IBaseRepository<ServiceSetting>
{
    public Task<ServiceSetting?> GetServiceSettingByNameAsync(string serviceName);
    public Task<bool> UpdateServiceInfoFromHealthReport(ServiceSetting serviceSetting);
    public Task<List<ServiceSetting>> GetAllInstances();
}