using BrandAndProductDatabase.API.Models.Response;
using FluentAssertions;

namespace BrandAndProductDatabase.API.Tests.Models.Response;

[TestClass]
public class BrandResponseTester
{
    [TestMethod]
    public void BrandResponse_SetAndGetId_ShouldSetAndReturnId()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var id = 1;

        // Act
        brandResponse.Id = id;
        var result = brandResponse.Id;

        // Assert
        result.Should().Be(id);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetName_ShouldSetAndReturnName()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var name = "Acme Co.";

        // Act
        brandResponse.Name = name;
        var result = brandResponse.Name;

        // Assert
        result.Should().Be(name);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetCountry_ShouldSetAndReturnCountry()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var country = "USA";

        // Act
        brandResponse.Country = country;
        var result = brandResponse.Country;

        // Assert
        result.Should().Be(country);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetEnvironmentRating_ShouldSetAndReturnEnvironmentRating()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var rating = 4;

        // Act
        brandResponse.EnvironmentRating = rating;
        var result = brandResponse.EnvironmentRating;

        // Assert
        result.Should().Be(rating);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetPeopleRating_ShouldSetAndReturnPeopleRating()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var rating = 3;

        // Act
        brandResponse.PeopleRating = rating;
        var result = brandResponse.PeopleRating;

        // Assert
        result.Should().Be(rating);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetAnimalRating_ShouldSetAndReturnAnimalRating()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var rating = 2;

        // Act
        brandResponse.AnimalRating = rating;
        var result = brandResponse.AnimalRating;

        // Assert
        result.Should().Be(rating);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetRatingDescription_ShouldSetAndReturnRatingDescription()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var description = "A description of the brand's rating.";

        // Act
        brandResponse.RatingDescription = description;
        var result = brandResponse.RatingDescription;

        // Assert
        result.Should().Be(description);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetCategories_ShouldSetAndReturnCategories()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var categories = new List<string> { "Shoes", "Clothing" };

        // Act
        brandResponse.Categories = categories;
        var result = brandResponse.Categories;

        // Assert
        result.Should().BeEquivalentTo(categories);
    }

    [TestMethod]
    public void BrandResponse_SetAndGetRanges_ShouldSetAndReturnRanges()
    {
        // Arrange
        var brandResponse = new BrandResponse();
        var ranges = new List<string> { "Spring/Summer", "Fall/Winter" };

        // Act
        brandResponse.Ranges = ranges;
        var result = brandResponse.Ranges;

        // Assert
        result.Should().BeEquivalentTo(ranges);
    }
}