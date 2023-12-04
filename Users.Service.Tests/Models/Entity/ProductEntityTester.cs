using FluentAssertions;
using Users.Service.Models.Entity;

namespace Users.Service.Tests.Models.Entity
{
    [TestClass]
    public class ProductEntityTester
    {
        [TestMethod]
        public void ProductEntity_Id_ShouldBeSettable()
        {
            // Arrange
            var product = new ProductEntity
            {
                // Act
                Id = 42
            };

            // Assert
            product.Id.Should().Be(42);
        }

        [TestMethod]
        public void ProductEntity_Name_ShouldBeSettable()
        {
            // Arrange
            var product = new ProductEntity
            {
                // Act
                Name = "Test Product"
            };

            // Assert
            product.Name.Should().Be("Test Product");
        }

        [TestMethod]
        public void ProductEntity_Rating_ShouldBeSettable()
        {
            // Arrange
            var product = new ProductEntity
            {
                // Act
                Rating = 5
            };

            // Assert
            product.Rating.Should().Be(5);
        }

        [TestMethod]
        public void ProductEntity_UserProductHistories_ShouldBeSettable()
        {
            // Arrange
            var product = new ProductEntity();
            var userProductHistories = new List<UserProductHistoryEntity>
            {
                new UserProductHistoryEntity { Id = 1, UserId = 1, ProductId = 1 },
                new UserProductHistoryEntity { Id = 2, UserId = 2, ProductId = 1 }
            };

            // Act
            product.UserProductHistories = userProductHistories;

            // Assert
            product.UserProductHistories.Should().NotBeNull().And.BeAssignableTo<List<UserProductHistoryEntity>>().And
                .BeEquivalentTo(userProductHistories);
        }
    }
}