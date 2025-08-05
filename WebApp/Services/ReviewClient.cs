using Frontend.DTOs.Review;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Frontend.Services;

public class ReviewClient(HttpClient httpClient, ProtectedSessionStorage sessionStorage)
{
    private async Task SetAuthHeaderAsync()
    {
        var result = await sessionStorage.GetAsync<string>("authToken");
        if (result.Success && !string.IsNullOrEmpty(result.Value))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Value);
        }
    }

    public async Task<IEnumerable<ReviewDto>?> GetReviewsByMovieAsync(int movieId)
        => await httpClient.GetFromJsonAsync<IEnumerable<ReviewDto>>($"api/Review/movie/{movieId}");

    public async Task<IEnumerable<ReviewDto>?> GetReviewsByUserAsync(int userId)
        => await httpClient.GetFromJsonAsync<IEnumerable<ReviewDto>>($"api/Review/user/{userId}");

    public async Task<ReviewDto?> CreateReviewAsync(CreateReviewDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PostAsJsonAsync("api/Review", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ReviewDto>();
    }

    public async Task<ReviewDto?> UpdateReviewAsync(int id, UpdateReviewDto dto)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.PutAsJsonAsync($"api/Review/{id}", dto);
        if (!response.IsSuccessStatusCode) return null;
        return await response.Content.ReadFromJsonAsync<ReviewDto>();
    }

    public async Task<bool> DeleteReviewAsync(int id)
    {
        await SetAuthHeaderAsync();
        var response = await httpClient.DeleteAsync($"api/Review/{id}");
        return response.IsSuccessStatusCode;
    }
}