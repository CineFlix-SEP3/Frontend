csharp
using Xunit;
using Moq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Frontend.DTOs;
using Frontend.Services;

namespace Frontend.Tests
{
    public class UserClientTests
    {
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserDto()
        {
            // Arrange
            var userId = 5;
            var expectedUser = new UserDto { Id = userId, Email = "test@example.com", Username = "testuser" };
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
                    Content = JsonContent.Create(expectedUser)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var sessionStorageMock = new Mock<ProtectedSessionStorage>(null);

            var client = new UserClient(httpClient, sessionStorageMock.Object);

            // Act
            var result = await client.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result!.Id);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.Username, result.Username);
        }
    }
}