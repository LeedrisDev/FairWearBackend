using BrandAndProduct.Service.Models.Entity;
using FluentAssertions;

namespace BrandAndProduct.Service.Tests.Models.Entity;

public class BrandEntityTester
{
    [TestClass]
    public class BrandEntityTests
    {
        [TestMethod]
        public void BrandEntity_Id_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                Id = 42
            };

            // Assert
            brand.Id.Should().Be(42);
        }

        [TestMethod]
        public void BrandEntity_Name_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                Name = "Test Brand"
            };

            // Assert
            brand.Name.Should().Be("Test Brand");
        }

        [TestMethod]
        public void BrandEntity_Country_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                Country = "Test Country"
            };

            // Assert
            brand.Country.Should().Be("Test Country");
        }

        [TestMethod]
        public void BrandEntity_EnvironmentRating_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                EnvironmentRating = 5
            };

            // Assert
            brand.EnvironmentRating.Should().Be(5);
        }

        [TestMethod]
        public void BrandEntity_PeopleRating_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                PeopleRating = 5
            };

            // Assert
            brand.PeopleRating.Should().Be(5);
        }

        [TestMethod]
        public void BrandEntity_AnimalRating_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                AnimalRating = 5
            };

            // Assert
            brand.AnimalRating.Should().Be(5);
        }

        [TestMethod]
        public void BrandEntity_RatingDescription_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity
            {
                // Act
                RatingDescription = "Test Description"
            };

            // Assert
            brand.RatingDescription.Should().Be("Test Description");
        }

        [TestMethod]
        public void BrandEntity_Categories_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity();
            var categories = new List<string> { "Category 1", "Category 2" };

            // Act
            brand.Categories = categories;

            // Assert
            brand.Categories.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(categories);
        }

        [TestMethod]
        public void BrandEntity_Ranges_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity();
            var ranges = new List<string> { "Range 1", "Range 2" };
            // Act
            brand.Ranges = ranges;

            // Assert
            brand.Ranges.Should().NotBeNull().And.BeAssignableTo<List<string>>().And.BeEquivalentTo(ranges);
        }

        [TestMethod]
        public void BrandEntity_Products_ShouldBeSettable()
        {
            // Arrange
            var brand = new BrandEntity();
            var products = new List<ProductEntity> { new ProductEntity { Id = 1 }, new ProductEntity { Id = 2 } };

            // Act
            brand.Products = products;

            // Assert
            brand.Products.Should().NotBeNull().And.BeAssignableTo<IEnumerable<ProductEntity>>().And
                .BeEquivalentTo(products);
        }
    }
}