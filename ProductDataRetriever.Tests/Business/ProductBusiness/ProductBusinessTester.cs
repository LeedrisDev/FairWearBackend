using System.Net;
using HtmlAgilityPack;
using Moq;
using ProductDataRetrieverAPI.DataAccess.ProductData;

namespace ProductDataRetriever.Test.Business.ProductBusiness;

[TestClass]
public class ProductBusinessTest
{
    private readonly Mock<IProductData> _productBusinessMock = new();
    
    [TestMethod]
    public async Task GetProductInformation()
    {
        _productBusinessMock.Setup(m => m.GetBarcodeInfoPageHtml(It.IsAny<string>()))
            .ReturnsAsync(GetTestHtmlDocument("SuccessData.html"));
        
        var productBusiness = new ProductDataRetrieverAPI.Business.ProductBusiness.ProductBusiness(_productBusinessMock.Object);
        var mock = _productBusinessMock.Object;
        var barcode = "193392069882";
        
        var result = await productBusiness.GetProductInformation(barcode);
   
        Assert.AreEqual(HttpStatusCode.OK, result.Status);
        Assert.AreEqual("The North Face", result.Object.BrandName);
    }
    
    public static HtmlDocument GetTestHtmlDocument(string testFileName)
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