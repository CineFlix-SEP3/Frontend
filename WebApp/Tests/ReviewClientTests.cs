using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Frontend.DTOs.Review;
using Frontend.Services;
using System.Collections.Generic;

namespace Frontend.Tests
{
    public class ReviewClientTests
    {
        [Fact]
        public async Task GetReviewsByMovieAsync_ReturnsReviewDtoList()
        {
            // Arrange
            var movieId = 1;
            var expectedReviews = new List<ReviewDto>
            {
                new ReviewDto { Id = 1, MovieId = movieId, Content = "Great movie!" },
                new ReviewDto { Id = 2, MovieId = movieId, Content = "Not bad." }
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
                    Content = JsonContent.Create(expectedReviews)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var sessionStorageMock = new Mock<ProtectedSessionStorage>(null);

            var client = new ReviewClient(httpClient, sessionStorageMock.Object);

            // Act
            var result = await client.GetReviewsByMovieAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedReviews.Count, ((List<ReviewDto>)result!).Count);
            Assert.Equal(expectedReviews[0].Content, ((List<ReviewDto>)result!)[0].Content);
        }
    }
}