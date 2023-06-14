using BrandAndProductDatabase.API.DataAccess;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BrandAndProductDatabase.API.Tests.DataAccess;

[TestClass]
public class BrandAndProductDbContextTester
{
    [TestMethod]
    public void CanInstantiateDbContext()
    {
        var options = new DbContextOptionsBuilder<BrandAndProductDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var dbContext = new BrandAndProductDbContext(options);

        dbContext.Should().NotBeNull();
    }
}