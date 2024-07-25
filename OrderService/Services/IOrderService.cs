using OrderService.Models;

namespace OrderService.Services;

// Interface that defines the contract for order-related operations
public interface IOrderService
{
    // Retrieves all orders asynchronously
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    // Retrieves an order by its ID asynchronously
    Task<Order> GetOrderByIdAsync(int id);

    // Adds a new order asynchronously
    Task AddOrderAsync(Order order);

    // Updates an existing order asynchronously
    Task UpdateOrderAsync(Order order);

    // Deletes an order by its ID asynchronously
    Task DeleteOrderAsync(int id);
}