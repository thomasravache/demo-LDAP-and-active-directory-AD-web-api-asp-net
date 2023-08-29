using DemoLDAPApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DemoLDAPApi.Controllers;

[ApiController]
[Route("[controller]")]
public class LDAPController : ControllerBase
{

    private readonly ILogger<LDAPController> _logger;
    private readonly LDAPService _service;

    public LDAPController(ILogger<LDAPController> logger, LDAPService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("GetCurrentDomainPath")]
    public IActionResult GetCurrentDomainPath()
    {
        var result = _service.GetCurrentDomainPath();

        return Ok(result);
    }

    [HttpGet("GetAllUsers")]
    public IActionResult GetAllUsers()
    {
        var result = _service.GetAllUsers();

        return Ok(result);
    }

    [HttpGet("GetAdditionalUserInfo")]
    public IActionResult GetAdditionalUserInfo()
    {
        var result = _service.GetAdditionalUserInfo();

        return Ok(result);
    }

    [HttpGet("SearchForUsers")]
    public IActionResult SearchForUsers([FromQuery] string userName)
    {
        var result = _service.SearchForUsers(userName);

        return Ok(result);
    }
}
