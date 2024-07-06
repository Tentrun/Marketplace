using Marketplace.BaseLibrary.Utils.Settings;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController(ILogger<SettingsController> logger) : ControllerBase
{
    private readonly ILogger<SettingsController> _logger = logger;

    [HttpGet(Name = "GetInstanceStatuses")]
    public async Task<IActionResult> GetInstanceStatuses()
    {
        return Ok(await SettingsBaseService.GetAllInstances());
    }
}