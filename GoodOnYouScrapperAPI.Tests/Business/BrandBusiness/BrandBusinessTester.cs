using GoodOnYouScrapperAPI.DataAccess.BrandData;
using HtmlAgilityPack;
using Moq;

namespace GoodOnYouScrapperAPI.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private readonly Mock<IBrandData> _brandDataMock = new();
    
    [TestInitialize]
    public void Startup()
    {
        _brandDataMock.Setup(m => m.GetBrandPageHtml(It.IsAny<string>()))
            .ReturnsAsync(getTestHtmlDocument());
    }
    
    [TestMethod]
    public async Task GetBrandInformations()
    {
        var brandBusiness = new GoodOnYouScrapperAPI.Business.BrandBusiness.BrandBusiness(_brandDataMock.Object);
        var mock = _brandDataMock.Object;
        var brandName = "Nike";

        var result = await brandBusiness.GetBrandInformation(brandName);
    }

    public static HtmlDocument getTestHtmlDocument()
    {
        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        
        if (projectDirectory == null)
            throw new Exception("Project directory not found");

        var filePath = Path.Combine(projectDirectory, "TestsData", "LevisData.html");

        var doc = new HtmlDocument();
        doc.Load(filePath);

        return doc;
    }
}