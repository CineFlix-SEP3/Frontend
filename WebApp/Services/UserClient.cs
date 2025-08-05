using Frontend.DTOs;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace Frontend.Services;

public class UserClient
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedSessionStorage _sessionStorage;

    public UserClient(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    private async Task SetAuthHeaderAsync()
    {
        var tokenResult = await _sessionStorage.GetAsync<string>("authToken");
        if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Value);
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<UserDto>($"api/User/{id}");
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<UserDto>($"api/User/email/{email}");
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<UserDto>($"api/User/username/{username}");
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/User/{id}", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/User/{id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<UserDto>?> GetAllUsersAsync()
    {
        await SetAuthHeaderAsync();
        return await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("api/User");
    }
}