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


}