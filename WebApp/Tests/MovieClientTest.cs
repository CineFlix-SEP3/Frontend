csharp
using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Frontend.DTOs.Movie;
using Frontend.Services;
using System.Collections.Generic;

namespace Frontend.Tests
{
    public class MovieClientTest
    {
        [Fact]
        public async Task GetMoviesByGenreAsync_ReturnsMovieDtoList()
        {
            // Arrange
            var genre = "Action";
            var expectedMovies = new List<MovieDto>
            {
                new MovieDto { Id = 1, Title = "Action Movie 1" },
                new MovieDto { Id = 2, Title = "Action Movie 2" }
            };
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(expectedMovies)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var sessionStorageMock = new Mock<ProtectedSessionStorage>(null);

            var client = new MovieClient(httpClient, sessionStorageMock.Object);

            // Act
            var result = await client.GetMoviesByGenreAsync(genre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovies.Count, ((List<MovieDto>)result!).Count);
            Assert.Equal(expectedMovies[0].Title, ((List<MovieDto>)result!)[0].Title);
        }
    }
}