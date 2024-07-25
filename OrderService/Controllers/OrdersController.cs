using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
// This controller handles CRUD operations for Order entities
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    // Constructor that injects the IOrderService dependency
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/orders
    // Retrieves all orders from the database
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    // GET: api/orders/{id}
    // Retrieves a specific order by its ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            // Returns 404 if the order is not found
            return NotFound();
        }
        // Returns 200 with the order data if found
        return Ok(order);
    }

    // POST: api/orders
    // Adds a new order to the database
    [HttpPost]
    public async Task<ActionResult> AddOrder(Order order)
    {
        try
        {
            await _orderService.AddOrderAsync(order);
            // Returns 201 with the URI of the newly created order
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if order data is invalid
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/orders/{id}
    // Updates an existing order by its ID
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateOrder(int id, Order order)
    {
        if (id != order.Id)
        {
            // Returns 400 if the URL ID does not match the order ID in the request body
            return BadRequest("Order ID mismatch");
        }

        try
        {
            await _orderService.UpdateOrderAsync(order);
            // Returns 204 if the update was successful
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if order data is invalid
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            // Returns 404 if order not found
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/orders/{id}
    // Deletes an order by its ID
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        try
        {
            await _orderService.DeleteOrderAsync(id);
            // Returns 204 if the delete was successful
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if order ID is invalid
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            // Returns 404 if order not found
            return NotFound(ex.Message);
        }
    }
}
