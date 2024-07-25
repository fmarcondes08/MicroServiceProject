using Microsoft.AspNetCore.Mvc;
using UserService.Controllers.Models;
using UserService.Services;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
// This controller handles CRUD operations for User entities
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    // Constructor that injects the IUserService dependency
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // GET: api/users
    // Retrieves all users from the database
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    // GET: api/users/{id}
    // Retrieves a specific user by their ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            // Returns 404 if the user is not found
            return NotFound();
        }
        // Returns 200 with the user data if found
        return Ok(user);
    }

    // POST: api/users
    // Adds a new user to the database
    [HttpPost]
    public async Task<ActionResult> AddUser(User user)
    {
        try
        {
            await _userService.AddUserAsync(user);
            // Returns 201 with the URI of the newly created user
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if user data is invalid
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/users/{id}
    // Updates an existing user by their ID
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            // Returns 400 if the URL ID does not match the user ID in the request body
            return BadRequest("User ID mismatch");
        }

        try
        {
            await _userService.UpdateUserAsync(user);
            // Returns 204 if the update was successful
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if user data is invalid
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            // Returns 404 if user not found
            return NotFound(ex.Message);
        }
    }

    // DELETE: api/users/{id}
    // Deletes a user by their ID
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await _userService.DeleteUserAsync(id);
            // Returns 204 if the delete was successful
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            // Returns 400 if user ID is invalid
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            // Returns 404 if user not found
            return NotFound(ex.Message);
        }
    }
}
