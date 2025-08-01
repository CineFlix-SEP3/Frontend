using Frontend.DTOs;
using Frontend.DTOs.Movie;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class MovieClient
{
    private readonly HttpClient _httpClient;
    private readonly ProtectedSessionStorage _sessionStorage;

    public MovieClient(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
    {
        _httpClient = httpClient;
        _sessionStorage = sessionStorage;
    }

    private async Task SetAuthHeaderAsync()
    {
        var result = await _sessionStorage.GetAsync<string>("authToken");
        Console.WriteLine($"Auth token exists: {result.Success && !string.IsNullOrEmpty(result.Value)}");
    
        if (result.Success && !string.IsNullOrEmpty(result.Value))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);
            Console.WriteLine($"Set Authorization header: Bearer {result.Value[..10]}...");
        }
        else
        {
            Console.WriteLine("No auth token found in session storage");
        }
    }

    public async Task<MovieDto?> GetMovieByIdAsync(int id)
        => await _httpClient.GetFromJsonAsync<MovieDto>($"api/Movie/{id}");

    public async Task<MovieDto?> GetMovieByTitleAsync(string title)
        => await _httpClient.GetFromJsonAsync<MovieDto>($"api/Movie/title/{title}");

    public async Task<IEnumerable<MovieDto>?> GetAllMoviesAsync()
        => await _httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>("api/Movie");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByGenreAsync(string genre)
        => await _httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/genre/{genre}");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByDirectorAsync(string director)
        => await _httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/director/{director}");

    public async Task<IEnumerable<MovieDto>?> GetMoviesByActorAsync(string actor)
        => await _httpClient.GetFromJsonAsync<IEnumerable<MovieDto>>($"api/Movie/actor/{actor}");

    public async Task<MovieDto?> CreateMovieAsync(CreateMovieDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PostAsJsonAsync("api/Movie", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<MovieDto?> UpdateMovieAsync(int id, UpdateMovieDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.PutAsJsonAsync($"api/Movie/{id}", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        await SetAuthHeaderAsync();
        var response = await _httpClient.DeleteAsync($"api/Movie/{id}");
        return response.IsSuccessStatusCode;
    }
}