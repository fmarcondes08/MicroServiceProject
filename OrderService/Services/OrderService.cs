using OrderService.Models;
using OrderService.Repositories;
using Shared.Messaging;
using Shared.Models;

namespace OrderService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMessageBusClient _messageBusClient;

    public OrderService(IOrderRepository orderRepository, IMessageBusClient messageBusClient)
    {
        _orderRepository = orderRepository;
        _messageBusClient = messageBusClient;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await _orderRepository.GetOrderByIdAsync(id);
    }

    public async Task AddOrderAsync(Order order)
    {
        if (order == null || string.IsNullOrWhiteSpace(order.Product) || order.Quantity <= 0 || order.UserId <= 0)
        {
            throw new ArgumentException("Order data is invalid");
        }
        
        await _orderRepository.AddOrderAsync(order);
        var orderPublishedDto = new Event
        {
            Id = order.Id,
            Product = order.Product,
            Quantity = order.Quantity,
            UserId = order.UserId
        };
        _messageBusClient.PublishNewOrder(orderPublishedDto);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        if (order == null || order.Id == 0 || string.IsNullOrWhiteSpace(order.Product) || order.Quantity <= 0 || order.UserId <= 0)
        {
            throw new ArgumentException("Order data is invalid");
        }

        if (!await OrderExistsAsync(order.Id))
        {
            throw new KeyNotFoundException("Order not found");
        }
        
        await _orderRepository.UpdateOrderAsync(order);
    }

    public async Task DeleteOrderAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Invalid order ID");
        }

        if (!await OrderExistsAsync(id))
        {
            throw new KeyNotFoundException("Order not found");
        }
        
        await _orderRepository.DeleteOrderAsync(id);
    }
    
    private async Task<bool> OrderExistsAsync(int id)
    {
        var order = await _orderRepository.GetOrderByIdAsync(id);
        return order != null;
    }
}