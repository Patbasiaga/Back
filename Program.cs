using Microsoft.AspNetCore.SignalR;
using SignalRWithMQTT_Back.Hubs;
using SignalRWithMQTT_Back.Services.MQTT;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddCors(corsOptions =>
{
  corsOptions.AddDefaultPolicy(policy =>
  {
    string frontHost = builder.Configuration.GetValue<string>("FrontHost");
    policy.WithOrigins(frontHost)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
  });
});


builder.Services.AddSingleton<MqttClientService>();
//builder.Services.AddSingleton<IHubContext<MqttHub>>();

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MqttHub>("/mqtt_hub");

app.Run();
