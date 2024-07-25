using Shared.Models;

namespace Shared.Messaging;

// Interface that defines the contract for message bus operations
public interface IMessageBusClient
{
    // Publishes a new order event to the message bus asynchronously
    Task PublishNewOrder(Event orderPublishedDto);
}