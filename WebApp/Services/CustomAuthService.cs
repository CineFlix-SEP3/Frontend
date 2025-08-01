using Frontend.DTOs;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class CustomAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _js;
    private readonly ProtectedSessionStorage _sessionStorage;

    public CustomAuthService(HttpClient httpClient, IJSRuntime js, ProtectedSessionStorage sessionStorage)
    {
        _httpClient = httpClient;
        _js = js;
        _sessionStorage = sessionStorage;
    }

    public class LoginResponse
    {
        public string Token { get; init; } = string.Empty;
    }

    public async Task<LoginResponse?> LoginAsync(string email, string password)
    {
        var request = new { Email = email, Password = password };
        var response = await _httpClient.PostAsJsonAsync("api/Auth/login", request);
        
        if (!response.IsSuccessStatusCode) return null;
        
        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
        
        if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
        {
            // Save token to session storage
            await _sessionStorage.SetAsync("authToken", loginResponse.Token);
            Console.WriteLine($"Token saved to session storage: {loginResponse.Token[..10]}...");
        }
        
        return loginResponse;
    }

    public async Task LogoutAsync()
    {
        await _sessionStorage.DeleteAsync("authToken");
        await _js.InvokeVoidAsync("sessionStorage.clear");
        await _js.InvokeVoidAsync("localStorage.clear");
    }

    public async Task<UserDto?> RegisterAsync(CreateUserRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/register", request);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}