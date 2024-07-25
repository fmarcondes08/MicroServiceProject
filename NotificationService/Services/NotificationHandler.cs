using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Shared.Models;

namespace NotificationService.Services;

// Background service that handles consuming messages from RabbitMQ
public class NotificationHandler : BackgroundService
{
    private readonly ILogger<NotificationHandler> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly RabbitMQSettings _rabbitMQSettings;

    // Constructor that initializes the RabbitMQ connection and channel
    public NotificationHandler(IOptions<RabbitMQSettings> rabbitMQSettings, IConfiguration configuration, ILogger<NotificationHandler> logger)
    {
        _logger = logger;
        _rabbitMQSettings = rabbitMQSettings.Value;
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.Host,
            Port = int.Parse(_rabbitMQSettings.Port),
            UserName = _rabbitMQSettings.UserName,
            Password = _rabbitMQSettings.Password,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };

        try
        {
            // Attempt to create a connection with retries
            _connection = CreateConnectionWithRetry(factory);
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

            _logger.LogInformation("Connected to RabbitMQ successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to connect to RabbitMQ: {ex.Message}");
            throw;
        }
    }
    
    // Method to create a connection with retry logic
    private IConnection CreateConnectionWithRetry(ConnectionFactory factory)
    {
        int retryAttempts = 5;
        int delayBetweenRetries = 5000; // milliseconds

        for (int attempt = 1; attempt <= retryAttempts; attempt++)
        {
            try
            {
                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                if (attempt == retryAttempts)
                {
                    throw;
                }
                _logger.LogWarning($"Failed to connect to RabbitMQ. Attempt {attempt} of {retryAttempts}. Retrying in {delayBetweenRetries / 1000} seconds...");
                Task.Delay(delayBetweenRetries).Wait();
            }
        }

        throw new Exception("Failed to create RabbitMQ connection after multiple attempts.");
    }
    
    // Main execution method for the background service
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        StartConsuming(stoppingToken);
        await Task.CompletedTask;
    }

    // Method to start consuming messages from the queue
    private void StartConsuming(CancellationToken cancellationToken)
    {
        _channel.QueueDeclare(queue: _rabbitMQSettings.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            bool processedSuccessfully = false;
            try
            {
                processedSuccessfully = await ProcessMessageAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred while processing message from queue {"orderQueue"}: {ex}");
            }

            if (processedSuccessfully)
            {
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            else
            {
                _channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
            }
        };

        _channel.BasicConsume(queue: _rabbitMQSettings.QueueName, autoAck: false, consumer: consumer);
    }
    
    // Method to process received messages
    private async Task<bool> ProcessMessageAsync(string message)
    {
        try
        {
            var orderPublishedDto = JsonSerializer.Deserialize<Event>(message);
                
            // Simulate sending notification
            _logger.LogInformation($"Order Received: {orderPublishedDto.Id}, {orderPublishedDto.Product}, {orderPublishedDto.Quantity}, {orderPublishedDto.UserId}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing message: {ex.Message}");
            return false;
        }
    }

    // Dispose method to close the channel and connection
    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
