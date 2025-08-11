using System.Net;
using Frontend.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace Frontend.Tests;

public class CustomAuthServiceTest
{
    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsTokenAndStoresIt()
    {
        // Arrange
        var expectedToken = "test-token";
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
                Content = JsonContent.Create(new CustomAuthService.LoginResponse { Token = expectedToken })
            });

        var httpClient = new HttpClient(handlerMock.Object);
        var jsMock = new Mock<IJSRuntime>();
        var sessionStorageMock = new Mock<ProtectedSessionStorage>(null);

        sessionStorageMock
            .Setup(x => x.SetAsync("authToken", expectedToken))
            .Returns(Task.CompletedTask);

        var service = new CustomAuthService(httpClient, jsMock.Object, sessionStorageMock.Object);

        // Act
        var result = await service.LoginAsync("user@example.com", "password");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedToken, result.Token);
        sessionStorageMock.Verify(x => x.SetAsync("authToken", expectedToken), Times.Once);
    }
}