using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using SignalRWithMQTT_Back.Hubs;

namespace SignalRWithMQTT_Back.Services.MQTT;

public class MqttClientService
{
  private IHubContext<MqttHub> _hubContext;
  private MqttClientOptions _mqttClientOptions;
  private IMqttClient _mqttClient;

  public MqttClientService(IHubContext<MqttHub> hubContext, IConfiguration configuration)
  {
    _hubContext = hubContext;

    MqttFactory mqttFactory = new MqttFactory();
    _mqttClient = mqttFactory.CreateMqttClient();
    string mqttBrokerAddress = configuration.GetValue<string>("MqttBrokerAddress");
    _mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(mqttBrokerAddress).Build();
    ConnectToMqtt();
    _mqttClient.ConnectedAsync += SubscribeToTopic;
  }

  public void ConnectToMqtt()
  {
    _ = Task.Run(
      async () =>
      {
        while (!_mqttClient.IsConnected)
        {
          try
          {
            MqttClientConnectResult mqttClientConnectResult
              = await _mqttClient.ConnectAsync(_mqttClientOptions, CancellationToken.None);
          }
          catch (Exception exception)
          {
            Console.WriteLine(exception.Message);
          }
          finally
          {
            await Task.Delay(TimeSpan.FromSeconds(5));
          }
        }
      });
  }

  public async Task SubscribeToTopic(MqttClientConnectedEventArgs mqttClientConnectedEventArgs)
  {
    MqttClientSubscribeResult mqttClientSubscribeResult
      = await _mqttClient.SubscribeAsync("REAKTO/telemetry/drone1");
    Console.WriteLine(mqttClientSubscribeResult.Items);
    _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceived;
  }

  public Task HandleMessageReceived(MqttApplicationMessageReceivedEventArgs mqttApplicationMessageReceivedEventArgs)
  {
    string? payloadMessage = mqttApplicationMessageReceivedEventArgs.ApplicationMessage.ConvertPayloadToString();
    if( payloadMessage is not null)
    {
      _hubContext.Clients.All.SendAsync("mqtt_frame", payloadMessage);
    }
    return Task.CompletedTask;  
  }

}
