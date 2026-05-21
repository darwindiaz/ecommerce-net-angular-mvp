using Ecommerce.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Tests.Persistence;

public class EcommerceDbContextTests
{
    [Fact]
    public void ShouldSeedInitialProducts()
    {
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<EcommerceDbContext>()
            .UseSqlite(connection)
            .Options;

        using var context = new EcommerceDbContext(options);
        context.Database.EnsureCreated();

        Assert.Equal(20, context.Products.Count());
    }
}
