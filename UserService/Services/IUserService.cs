using UserService.Controllers.Models;

namespace UserService.Services;

// Interface that defines the contract for user-related operations
public interface IUserService
{
    // Retrieves all users asynchronously
    Task<IEnumerable<User>> GetAllUsersAsync();

    // Retrieves a user by their ID asynchronously
    Task<User> GetUserByIdAsync(int id);

    // Adds a new user asynchronously
    Task AddUserAsync(User user);

    // Updates an existing user asynchronously
    Task UpdateUserAsync(User user);

    // Deletes a user by their ID asynchronously
    Task DeleteUserAsync(int id);
}