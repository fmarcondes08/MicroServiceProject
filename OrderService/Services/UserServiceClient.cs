namespace OrderService.Services;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> UserExistsAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://userservice/api/users/{userId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            // Log error (not implemented in this example)
            return false;
        }
    }
}