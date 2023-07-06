using FairWearProductDataRetriever.Service.Models;

namespace ProductDataRetriever.Test.Models;

[TestClass]
public class ProductModelTester
{
    [TestMethod]
    public void Create_ProductModel_With_UpcCode()
    {
        var product = new ProductModel
        {
            UpcCode = "193392069882"
        };

        Assert.AreEqual("193392069882", product.UpcCode);
    }

    [TestMethod]
    public void Create_ProductModel_With_Name()
    {
        var product = new ProductModel
        {
            Name = "North face gloves"
        };

        Assert.AreEqual("North face gloves", product.Name);
    }

    [TestMethod]
    public void Create_ProductModel_With_BrandName()
    {
        var product = new ProductModel
        {
            BrandName = "North face"
        };

        Assert.AreEqual("North face", product.BrandName);
    }

    [TestMethod]
    public void Create_ProductModel_With_Category()
    {
        var product = new ProductModel
        {
            Category = "Gloves"
        };

        Assert.AreEqual("Gloves", product.Category);
    }

    [TestMethod]
    public void Create_ProductModel_With_Ranges()
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