using UserService.Controllers.Models;

namespace UserService.Repositories;

// Interface that defines the contract for user-related database operations
public interface IUserRepository
{
    // Retrieves all users from the database asynchronously
    Task<IEnumerable<User>> GetAllUsersAsync();

    // Retrieves a user by their ID from the database asynchronously
    Task<User> GetUserByIdAsync(int id);

    // Adds a new user to the database asynchronously
    Task AddUserAsync(User user);

    // Updates an existing user in the database asynchronously
    Task UpdateUserAsync(User user);

    // Deletes a user by their ID from the database asynchronously
    Task DeleteUserAsync(int id);
}