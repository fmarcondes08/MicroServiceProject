using OrderService.Models;

namespace OrderService.Repositories;

// Interface that defines the contract for order-related database operations
public interface IOrderRepository
{
    // Retrieves all orders from the database asynchronously
    Task<IEnumerable<Order>> GetAllOrdersAsync();

    // Retrieves an order by its ID from the database asynchronously
    Task<Order> GetOrderByIdAsync(int id);

    // Adds a new order to the database asynchronously
    Task AddOrderAsync(Order order);

    // Updates an existing order in the database asynchronously
    Task UpdateOrderAsync(Order order);

    // Deletes an order by its ID from the database asynchronously
    Task DeleteOrderAsync(int id);
}