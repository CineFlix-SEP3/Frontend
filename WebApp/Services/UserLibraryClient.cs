using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace Frontend.Services;

public class UserLibraryClient(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
{
    private async Task SetAuthHeaderAsync()
    {
        var result = await sessionStorage.GetAsync<string>("authToken");
        if (result.Success && !string.IsNullOrEmpty(result.Value))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);
        }
    }

    public async Task<bool> AddFavoriteAsync(int movieId)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PostAsync($"api/UserLibrary/favorite/{movieId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> AddWatchListAsync(int movieId)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PostAsync($"api/UserLibrary/watchlist/{movieId}", null);
        return response.IsSuccessStatusCode;
    }

    public async Task<IEnumerable<int>?> GetFavoritesAsync()
    {
        await SetAuthHeaderAsync();
        return await httpClient.GetFromJsonAsync<IEnumerable<int>>("api/UserLibrary/favorite");
    }

    public async Task<IEnumerable<int>?> GetWatchListAsync()
    {
        await SetAuthHeaderAsync();
        return await httpClient.GetFromJsonAsync<IEnumerable<int>>("api/UserLibrary/watchlist");
    }

    public async Task<bool> RemoveFavoriteAsync(int movieId)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.DeleteAsync($"api/UserLibrary/favorite/{movieId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> RemoveWatchListAsync(int movieId)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.DeleteAsync($"api/UserLibrary/watchlist/{movieId}");
        return response.IsSuccessStatusCode;
    }
}