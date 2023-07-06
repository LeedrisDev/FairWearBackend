using System.Net;
using FairWearProductDataRetriever.API.DataAccess.ProductData;
using HtmlAgilityPack;
using Moq;

namespace ProductDataRetriever.Test.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTest
{
    private readonly Mock<IProductData> _productBusinessMock = new();

    [TestMethod]
    public async Task GetProductInformation()
    {
        _productBusinessMock.Setup(m => m.GetBarcodeInfoPageHtml(It.IsAny<string>()))
            .ReturnsAsync(GetTestHtmlDocument("LevisProductData.html"));

        var productBusiness =
            new FairWearProductDataRetriever.API.Business.ProductBusiness.ProductBusiness(_productBusinessMock
                .Object);
        var barcode = "3665115029871";

        var result = await productBusiness.GetProductInformation(barcode);

        Assert.AreEqual(HttpStatusCode.OK, result.Status);
        Assert.AreEqual("Levi's", result.Object.BrandName);
        Assert.AreEqual("Levi's White T-shirt For With Logo", result.Object.Name);
        Assert.AreEqual("Shirts & Tops", result.Object.Category);
    }

    [TestMethod]
    public async Task GetProductInformation_WithWrongBarcode_ThenReturnsNotFound()
    {
        _productBusinessMock.Setup(m => m.GetBarcodeInfoPageHtml(It.IsAny<string>()))
            .ReturnsAsync(GetTestHtmlDocument("NotFoundData.html"));

        var productBusiness =
            new FairWearProductDataRetriever.API.Business.ProductBusiness.ProductBusiness(_productBusinessMock
                .Object);
        var barcode = "3102271165352";

        var result = await productBusiness.GetProductInformation(barcode);

        Assert.AreEqual(HttpStatusCode.NotFound, result.Status);
        Assert.AreEqual("Product not found", result.ErrorMessage);
    }

    private static HtmlDocument GetTestHtmlDocument(string testFileName)
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;

        if (projectDirectory == null)
            throw new Exception("Project directory not found");

        var filePath = Path.Combine(projectDirectory, "TestsData", testFileName);
        var doc = new HtmlDocument();
        doc.Load(filePath);

        return doc;
    }
}