using Frontend.DTOs;

namespace Frontend.Services;

public class UserClient(System.Net.Http.HttpClient httpClient)
{
    public async Task<UserDto?> CreateUserAsync(CreateUserRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/Auth/register", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<UserDto>($"api/User/{id}");
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        return await httpClient.GetFromJsonAsync<UserDto>($"api/User/email/{email}");
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        return await httpClient.GetFromJsonAsync<UserDto>($"api/User/username/{username}");
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        var response = await httpClient.PutAsJsonAsync($"api/User/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/User/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<UserDto>?> GetAllUsersAsync()
    {
        return await httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("api/User");
    }
}