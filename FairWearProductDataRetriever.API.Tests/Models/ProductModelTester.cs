using FairWearProductDataRetriever.API.Models;

namespace ProductDataRetriever.Test.Models;

[TestClass]
public class ProductModelTester
{
    [TestMethod]
    public void TestUpcCode()
    {
        var product = new ProductModel
        {
            UpcCode = "193392069882"
        };

        Assert.AreEqual("193392069882", product.UpcCode);
    }
    
    [TestMethod]
    public void TestName()
    {
        var product = new ProductModel
        {
            Name = "North face gloves"
        };

        Assert.AreEqual("North face gloves", product.Name);
    }

    [TestMethod]
    public void TestBrandName()
    {
        var product = new ProductModel
        {
            BrandName = "North face"
        };

        Assert.AreEqual("North face", product.BrandName);
    }

    [TestMethod]
    public void TestCategory()
    {
        var product = new ProductModel
        {
            Category = "Gloves"
        };

        Assert.AreEqual("Gloves", product.Category);
    }

    [TestMethod]
    public void TestRanges()
    {
        var product = new ProductModel
        {
            Ranges = new List<string>
            {
                "Men", "Women"
            }
        };

        Assert.AreEqual("Men", product.Ranges.ElementAt(0));
        Assert.AreEqual("Women", product.Ranges.ElementAt(1));
    }

}