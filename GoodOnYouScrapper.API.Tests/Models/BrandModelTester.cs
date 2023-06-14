using GoodOnYouScrapper.API.Models;
using GoodOnYouScrapper.API.Models.Response;

namespace GoodOnYouScrapper.API.Tests.Models;

[TestClass]
public class BrandModelTester
{
    [TestMethod]
    public void Create_BrandModel_With_Name()
    {
        var brand = new BrandResponse
        {
            Name = "Levis"
        };

        Assert.AreEqual("Levis", brand.Name);
    }

    
    [TestMethod]
    public void Create_BrandModel_With_Country()
    {
        var brand = new BrandResponse
        {
            Country = "United States"
        };

        Assert.AreEqual("United States", brand.Country);
    }
    
    
    [TestMethod]
    public void Create_BrandModel_With_EnvironmentRating()
    {
        var brand = new BrandResponse
        {
            EnvironmentRating = 1
        };

        Assert.AreEqual(1, brand.EnvironmentRating);
    }
    
    
    [TestMethod]
    public void Create_BrandModel_With_PeopleRating()
    {
        var brand = new BrandResponse
        {
            PeopleRating = 1
        };

        Assert.AreEqual(1, brand.PeopleRating);
    }
    
    
    [TestMethod]
    public void Create_BrandModel_With_AnimalRating()
    {
        var brand = new BrandResponse
        {
            AnimalRating = 1
        };

        Assert.AreEqual(1, brand.AnimalRating);
    }
    
    
    [TestMethod]
    public void Create_BrandModel_With_RatingDescription()
    {
        var brand = new BrandResponse
        {
            RatingDescription = "Description"
        };

        Assert.AreEqual("Description", brand.RatingDescription);
    }

    [TestMethod]
    public void Create_BrandModel_With_Categories()
    {
        var brand = new BrandResponse
        {
            Categories = new List<string>
            {
                "Shoes", "Shirts"
            }
        };

        Assert.AreEqual("Shoes", brand.Categories.ElementAt(0));
        Assert.AreEqual("Shirts", brand.Categories.ElementAt(1));
    }
    
    
    [TestMethod]
    public void Create_BrandModel_With_Ranges()
    {
        var brand = new BrandResponse
        {
            Ranges = new List<string>
            {
                "Men", "Women"
            }
        };

        Assert.AreEqual("Men", brand.Ranges.ElementAt(0));
        Assert.AreEqual("Women", brand.Ranges.ElementAt(1));
    }
}