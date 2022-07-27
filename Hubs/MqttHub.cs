using Microsoft.AspNetCore.SignalR;
using SignalRWithMQTT_Back.Services.MQTT;

namespace SignalRWithMQTT_Back.Hubs;

public class MqttHub : Hub
{

  public MqttHub(MqttClientService mqttClientService)
  {
    
  }

}
