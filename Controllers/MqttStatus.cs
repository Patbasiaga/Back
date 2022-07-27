using Microsoft.AspNetCore.Mvc;

namespace SignalRWithMQTT_Back.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MqttStatus : ControllerBase
{
  private readonly ILogger<MqttStatus> _logger;

  public MqttStatus(ILogger<MqttStatus> logger)
  {
    _logger = logger;
  }

  [HttpGet(Name = "endpoint_status")]
  public IActionResult Get()
  {
    return Ok("MQTT endpoint working");
  }
}
