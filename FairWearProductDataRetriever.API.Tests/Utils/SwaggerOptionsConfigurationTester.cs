using FairWearProductDataRetriever.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Moq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductDataRetriever.Test.Utils;

[TestClass]
public class SwaggerOptionsConfigurationTester
{
    [TestMethod]
    public void Configure_ShouldAddSwaggerDocumentForEachApiVersion()
    {
        var providerMock = new Mock<IApiVersionDescriptionProvider>();
        var options = new SwaggerGenOptions();
        var apiVersionDescriptions = new List<ApiVersionDescription>
        {
            new(new ApiVersion(1, 0), "v1", true),
            new(new ApiVersion(2, 0), "v2", false)
        };
        providerMock.Setup(p => p.ApiVersionDescriptions).Returns(apiVersionDescriptions);

        var swaggerOptionsConfiguration = new SwaggerOptionsConfiguration(providerMock.Object);

        swaggerOptionsConfiguration.Configure(options);

        Assert.AreEqual(2, options.SwaggerGeneratorOptions.SwaggerDocs.Count);
        Assert.IsTrue(options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey("v1"));
        Assert.IsTrue(options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey("v2"));

        var v1Info = options.SwaggerGeneratorOptions.SwaggerDocs["v1"];
        Assert.AreEqual(AppConstants.ApiName, v1Info.Title);
        Assert.AreEqual("1.0", v1Info.Version);
        Assert.AreEqual($" {AppConstants.ApiVersionDeprecatedDescription}", v1Info.Description);

        var v2Info = options.SwaggerGeneratorOptions.SwaggerDocs["v2"];
        Assert.AreEqual(AppConstants.ApiName, v2Info.Title);
        Assert.AreEqual("2.0", v2Info.Version);
    }
}