namespace OrderService.Services;

public interface IUserServiceClient
{
    Task<bool> UserExistsAsync(int userId);
}