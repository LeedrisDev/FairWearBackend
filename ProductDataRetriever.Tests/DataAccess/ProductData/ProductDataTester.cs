using System.Net;
using FluentAssertions;
using HtmlAgilityPack;
using Moq;
using ProductDataRetrieverAPI.Utils.HttpClientWrapper;

namespace ProductDataRetriever.Test.DataAccess.ProductData;

[TestClass]
public class ProductDataTester
{
    private readonly Mock<HtmlDocument> _htmlDocumentMock = new();
    private readonly Mock<IHttpClientWrapper> _httpClientMock = new();
    
    [TestMethod]
    public async Task GetBarcodeInfoPageHtml_WithBarcode_ThenResultIsNotNull()
    {
        const string barcode = "193392069882";
        var httpClient = new HttpClientWrapper(new HttpClient());
        var productData = new ProductDataRetrieverAPI.DataAccess.ProductData.ProductData(httpClient, _htmlDocumentMock.Object);
        
        var result = await productData.GetBarcodeInfoPageHtml(barcode);
        result.Text
            .Should()
            .NotBeNullOrEmpty();
    }
    
    [TestMethod]
    public async Task GetBarcodeInfoPageHtml_WithBarcode_Async_ThenShouldNotThrowException()
    {
        _httpClientMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(await GetBarcodeInfoPage("SuccessData.html"))
            });
        
        const string barcode = "193392069882";
        var productData = new ProductDataRetrieverAPI.DataAccess.ProductData.ProductData(_httpClientMock.Object, _htmlDocumentMock.Object);

        await productData
            .Invoking(m => m.GetBarcodeInfoPageHtml(barcode))
            .Should()
            .NotThrowAsync<HttpRequestException>();
    }
    
    [TestMethod]
    public async Task GetBarcodeInfoPageHtml_WithWrongBarcode_ThenTrowsException()
    {
        _httpClientMock.Setup(m => m.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = new StringContent(await GetBarcodeInfoPage("NotFoundData.html"))
            });
        
        const string barcode = "203392069882";
        var productData = new ProductDataRetrieverAPI.DataAccess.ProductData.ProductData(_httpClientMock.Object, _htmlDocumentMock.Object);

        await productData
            .Invoking(m => m.GetBarcodeInfoPageHtml(barcode))
            .Should()
            .ThrowAsync<HttpRequestException>();
    }
    
    private static async Task<string> GetBarcodeInfoPage(string filename)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        
        if (projectDirectory == null)
            throw new Exception("Project directory not found");

        var filePath = Path.Combine(projectDirectory, "TestsData", filename);
        var html = await File.ReadAllTextAsync(filePath);
        
        return html;
    }
}