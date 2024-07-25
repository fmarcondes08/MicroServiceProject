using UserService.Controllers.Models;
using UserService.Repositories;

namespace UserService.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        if (user == null || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("User data is invalid");
        }
        
        await _userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        if (user == null || user.Id == 0 || string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("User data is invalid");
        }

        if (!await UserExistsAsync(user.Id))
        {
            throw new KeyNotFoundException("User not found");
        }
        
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        if (id == 0)
        {
            throw new ArgumentException("Invalid user ID");
        }

        if (!await UserExistsAsync(id))
        {
            throw new KeyNotFoundException("User not found");
        }
        
        await _userRepository.DeleteUserAsync(id);
    }
    
    private async Task<bool> UserExistsAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user != null;
    }
}