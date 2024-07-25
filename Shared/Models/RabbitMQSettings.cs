namespace Shared.Models;

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueueName { get; set; }
}