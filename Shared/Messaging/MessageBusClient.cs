using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Models;

namespace Shared.Messaging;

public class MessageBusClient : IMessageBusClient, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMQSettings _rabbitMQSettings;

    public MessageBusClient(IOptions<RabbitMQSettings> rabbitMQSettings, IConfiguration configuration)
    {
        _rabbitMQSettings = rabbitMQSettings.Value;
    }

    public async Task PublishNewOrder(Event eventDto)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMQSettings.Host,
            Port = int.Parse(_rabbitMQSettings.Port),
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password,
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: _rabbitMQSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var message = JsonSerializer.Serialize(eventDto);
        var body = Encoding.UTF8.GetBytes(message);
        await Task.Run(() => channel.BasicPublish(exchange: "", routingKey: _rabbitMQSettings.QueueName, basicProperties: null, body: body));
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

    public void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
}