using Frontend.DTOs;
using Frontend.DTOs.Movie;

namespace Frontend.Services;

public class MovieClient(HttpClient httpClient)
{
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
        var response = await httpClient.PostAsJsonAsync("api/Movie", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<MovieDto?> UpdateMovieAsync(int id, UpdateMovieDto dto)
    {
        var response = await httpClient.PutAsJsonAsync($"api/Movie/{id}", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<MovieDto>();
    }

    public async Task<bool> DeleteMovieAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/Movie/{id}");
        return response.IsSuccessStatusCode;
    }
}