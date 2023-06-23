using FairWearGateway.API.Models.Response;
using FluentAssertions;

namespace FairWearGateway.API.Tests.Models.Response;

[TestClass]
public class BrandResponseTester
{
    [TestMethod]
    public void BrandResponse_Id_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.Id = 1;

        // Assert
        brandResponse.Id.Should().Be(1);
    }

    [TestMethod]
    public void BrandResponse_Name_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.Name = "Nike";

        // Assert
        brandResponse.Name.Should().Be("Nike");
    }

    [TestMethod]
    public void BrandResponse_Country_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.Country = "USA";

        // Assert
        brandResponse.Country.Should().Be("USA");
    }

    [TestMethod]
    public void BrandResponse_EnvironmentRating_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.EnvironmentRating = 4;

        // Assert
        brandResponse.EnvironmentRating.Should().Be(4);
    }

    [TestMethod]
    public void BrandResponse_PeopleRating_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.PeopleRating = 3;

        // Assert
        brandResponse.PeopleRating.Should().Be(3);
    }

    [TestMethod]
    public void BrandResponse_AnimalRating_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.AnimalRating = 5;

        // Assert
        brandResponse.AnimalRating.Should().Be(5);
    }

    [TestMethod]
    public void BrandResponse_RatingDescription_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();

        // Act
        brandResponse.RatingDescription = "High rating";

        // Assert
        brandResponse.RatingDescription.Should().Be("High rating");
    }

    [TestMethod]
    public void BrandResponse_Categories_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var categories = new List<string> { "Category1", "Category2" };

        // Act
        brandResponse.Categories = categories;

        // Assert
        brandResponse.Categories.Should().BeEquivalentTo(categories);
    }

    [TestMethod]
    public void BrandResponse_Ranges_ShouldBeSettable()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var ranges = new List<string> { "Range1", "Range2" };

        // Act
        brandResponse.Ranges = ranges;

        // Assert
        brandResponse.Ranges.Should().BeEquivalentTo(ranges);
    }
}