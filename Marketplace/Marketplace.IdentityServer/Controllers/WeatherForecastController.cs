using Marketplace.BaseLibrary.Dto.Identity;
using Marketplace.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IIdentityService _identityService;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IUserService userService, IIdentityService identityService)
    {
        _logger = logger;
        _userService = userService;
        _identityService = identityService;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        var request = new RequestRegisterUserDto("TestName", "TestSecondName", "TestPatro",
            "test@test.test", "+79017079493","test123@");
        var result = await _userService.RegisterUser(request);

        return Ok(result);
    }
    
    [HttpGet("Test2")]
    public async Task<IActionResult> Test2(string data, string pass)
    {
        var user = await _userService.GetUser(data);
        var isValid = await _identityService.AuthenticateUser(user, pass);
        if (!isValid)
        {
            return Unauthorized("Данные не валид");
        }
        var result = await _identityService.AuthorizeUser(user);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("Test3")]
    public async Task<IActionResult> Test3()
    {
        return Ok("work");
    }
}