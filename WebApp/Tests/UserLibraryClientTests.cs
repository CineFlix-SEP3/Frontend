using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Frontend.Services;

namespace Frontend.Tests
{
    public class UserLibraryClientTests
    {
        [Fact]
        public async Task AddFavoriteAsync_ReturnsTrueOnSuccess()
        {
            // Arrange
            var movieId = 10;
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var sessionStorageMock = new Mock<ProtectedSessionStorage>(null);

            var client = new UserLibraryClient(httpClient, sessionStorageMock.Object);

            // Act
            var result = await client.AddFavoriteAsync(movieId);

            // Assert
            Assert.True(result);
        }
    }
}