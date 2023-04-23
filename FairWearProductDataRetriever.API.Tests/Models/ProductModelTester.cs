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
    
    


}