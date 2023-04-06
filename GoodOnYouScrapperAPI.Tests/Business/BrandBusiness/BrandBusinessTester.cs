using GoodOnYouScrapperAPI.DataAccess.BrandData;
using HtmlAgilityPack;
using Moq;

namespace GoodOnYouScrapperAPI.Tests.Business.BrandBusiness;

[TestClass]
public class BrandBusinessTester
{
    private readonly Mock<IBrandData> _brandDataMock = new();

    [TestMethod]
    [DataRow("LevisData.html", 4, 2, 2)]
    [DataRow("NikeData.html", 3, 3, 2)]
    [DataRow("UniqloData.html", 3, 3, 2)]
    public async Task GetBrandInformations(string fileName, int environmentRating, int peopleRating, int animalRating)
    {
        _brandDataMock.Setup(m => m.GetBrandPageHtml(It.IsAny<string>()))
            .ReturnsAsync(GetTestHtmlDocument(fileName));
        
        var brandBusiness = new GoodOnYouScrapperAPI.Business.BrandBusiness.BrandBusiness(_brandDataMock.Object);
        var mock = _brandDataMock.Object;
        var brandName = "Levis";

        var result = await brandBusiness.GetBrandInformation(brandName);
        Assert.AreEqual(environmentRating, result.Object.EnvironmentRating);
        Assert.AreEqual(peopleRating, result.Object.PeopleRating);
        Assert.AreEqual(animalRating, result.Object.AnimalRating);
        Assert.IsFalse(string.IsNullOrEmpty(result.Object.RatingDescription));
        Assert.IsFalse(string.IsNullOrEmpty(result.Object.Country));
        Assert.IsTrue(result.Object?.Categories?.Length > 0);
        Assert.IsTrue(result.Object?.Ranges?.Length > 0);
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