using Frontend.DTOs.Movie;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class MovieClient(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
{
    private async Task SetAuthHeaderAsync()
    {
        var result = await sessionStorage.GetAsync<string>("authToken");
        if (result.Success && !string.IsNullOrEmpty(result.Value))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);
        }
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
        => await httpClient.GetFromJsonAsync<MovieDto>($"api/Movie/{id}");

    public async Task<MovieDto?> GetMovieByTitleAsync(string title)
        => await httpClient.GetFromJsonAsync<MovieDto>($"api/Movie/title/{title}");

    public async Task<IEnumerable<MovieDto>?> GetAllMoviesAsync()
        => await httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>("api/Movie");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByGenreAsync(string genre)
        => await httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/genre/{genre}");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByDirectorAsync(string director)
        => await httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/director/{director}");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByActorAsync(string actor)
        => await httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/actor/{actor}");

    public async Task<MovieDto?> CreateMovieAsync(CreateMovieDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PostAsJsonAsync("api/Movie", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<MovieDto?> UpdateMovieAsync(int id, UpdateMovieDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PutAsJsonAsync($"api/Movie/{id}", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.DeleteAsync($"api/Movie/{id}");
        return response.IsSuccessStatusCode;
    }
}