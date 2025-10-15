using Microsoft.AspNetCore.Mvc;
using DataServiceLayer;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly DataService _dataService;

    public TestController()
    {
        _dataService = new DataService();
    }

    [HttpGet("welcome")]
    public IActionResult GetWelcome()
    {
        var message = _dataService.GetWelcomeMessage();
        return Ok(new { message });
    }

    [HttpGet("data")]
    public IActionResult GetData()
    {
        var data = _dataService.GetTestData();
        return Ok(data);
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new 
        { 
            status = "OK", 
            timestamp = DateTime.Now,
            service = "WebServiceLayer"
        });
    }
}
