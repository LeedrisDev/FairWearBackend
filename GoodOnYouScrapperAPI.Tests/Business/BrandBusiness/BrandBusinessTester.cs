using GoodOnYouScrapperAPI.DataAccess.BrandData;
using HtmlAgilityPack;
using Moq;

namespace GoodOnYouScrapperAPI.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private readonly Mock<IBrandData> _brandDataMock = new();

    [TestMethod]
    public async Task GetBrandInformations()
    {
        _brandDataMock.Setup(m => m.GetBrandPageHtml(It.IsAny<string>()))
            .ReturnsAsync(GetTestHtmlDocument());
        
        var brandBusiness = new GoodOnYouScrapperAPI.Business.BrandBusiness.BrandBusiness(_brandDataMock.Object);
        var mock = _brandDataMock.Object;
        var brandName = "Levis";

        var result = await brandBusiness.GetBrandInformation(brandName);
        
        Assert.AreEqual(4, result.Object.EnvironmentRating);
        Assert.AreEqual(2, result.Object.PeopleRating);
        Assert.AreEqual(2, result.Object.AnimalRating);
    }
    
    public static HtmlDocument GetTestHtmlDocument()
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