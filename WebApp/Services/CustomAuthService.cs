using Frontend.DTOs;
using Microsoft.JSInterop;

namespace Frontend.Services;

public class CustomAuthService(HttpClient httpClient, IJSRuntime js)
{
    public class LoginResponse
    {
        public string Token { get; init; } = string.Empty;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var request = new { Email = email, Password = password };
        var response = await httpClient.PostAsJsonAsync("api/Auth/login", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task LogoutAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "jwt");
        await js.InvokeVoidAsync("sessionStorage.clear");
        await js.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<UserDto?> RegisterAsync(CreateUserRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("api/Auth/register", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}