using Marketplace.BaseLibrary.Dto;
using Marketplace.BaseLibrary.Utils.Base.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
    [HttpGet("GetInstanceStatuses")]
    public async Task<List<ServiceInstanceDTO>> GetProductsOfTheDay()
    {
        var result = await SettingsBaseService.GetAllInstances();

        return result;
    }
}