using System.Net;
using FluentAssertions;
using Moq;

namespace BrandAndProduct.Service.Tests.Utils.HttpClientWrapper;

[TestClass]
public class HttpClientWrapperTester
{
    private readonly Mock<HttpClient> _httpClientMock = new();
    
    [TestMethod]
    public async Task GetAsync_WithGoogleUrl_ThenStatusCodeIsOK()
    {
        // Arrange
        const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        var httpClientWrapper = new BrandAndProduct.Service.Utils.HttpClientWrapper.HttpClientWrapper(_httpClientMock.Object);
        const string requestUri = "https://www.google.com";
        
        // Act
        var result = await httpClientWrapper.GetAsync(requestUri);
        
        // Assert
        result.StatusCode.Should().Be(expectedStatusCode);
    }
    
    [TestMethod]
    public async Task GetAsync_WithGoogleUrl_ThenContentIsNotNull()
    {
        // Arrange
        var httpClientWrapper = new BrandAndProduct.Service.Utils.HttpClientWrapper.HttpClientWrapper(_httpClientMock.Object);
        const string requestUri = "https://www.google.com";
        
        // Act
        var result = await httpClientWrapper.GetAsync(requestUri);
        
        // Assert
        result.Content.Should().NotBeNull();
    }
}