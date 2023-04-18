using System.Net;
using FluentAssertions;
using GoodOnYouScrapperAPI.DataAccess.BrandData;
using GoodOnYouScrapperAPI.Utils.HttpClientWrapper;
using HtmlAgilityPack;
using Moq;

namespace GoodOnYouScrapperAPI.Tests.DataAccess;

[TestClass]
public class BrandDataTester
{
    private readonly Mock<HtmlDocument> _htmlDocumentMock = new();
    private readonly Mock<IHttpClientWrapper> _httpClientMock = new();

    [TestMethod]
    public async Task GetBrandPageHtml_WithBrandNameAndGetTheSiteHtml_ThenResultIsNotNull()
    {
        const string brandName = "levis";
        var httpClient = new HttpClientWrapper(new HttpClient());
        var brandData = new BrandData(httpClient, _htmlDocumentMock.Object);
        
        var result = await brandData.GetBrandPageHtml(brandName);

        result.Text
            .Should()
            .NotBeNullOrEmpty();
    }
    
    [TestMethod]
    public async Task GetBrandPageHtml_WithBrandName_ThenResultIsNotNull()
    {
        _httpClientMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(await GetBrandPageHtml())
            });
        const string brandName = "levis";
        var brandData = new BrandData(_httpClientMock.Object, _htmlDocumentMock.Object);
        
        var result = await brandData.GetBrandPageHtml(brandName);

        result.Text
            .Should()
            .NotBeNullOrEmpty();
    }

    [TestMethod]
    public async Task GetBrandPageHtml_WithFakeBrandName_ThenTrowsException()
    {
        _httpClientMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(await GetBrandPageHtml())
            });
        
        const string brandName = "fakeBrandName";
        var brandData = new BrandData(_httpClientMock.Object, _htmlDocumentMock.Object);

        await brandData
            .Invoking(m => m.GetBrandPageHtml(brandName))
            .Should()
            .ThrowAsync<HttpRequestException>();
    }

    private static async Task<string> GetBrandPageHtml()
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        
        if (projectDirectory == null)
            throw new Exception("Project directory not found");

        var filePath = Path.Combine(projectDirectory, "TestsData", "LevisData.html");
        var html = await File.ReadAllTextAsync(filePath);
        
        return html;
    }
}