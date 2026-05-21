using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Enums;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Tests.Products;

public class ProductServiceTests
{
    [Fact]
    public async Task ShouldCreateProduct()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();

        var service = CreateProductService(context);
        var request = CreateProductRequest("SNK-WHT-07-999");

        var product = await service.CreateAsync(request);

        Assert.Equal("SNK-WHT-07-999", product.Code);
        Assert.True(product.IsAvailable);
        Assert.Equal(21, context.Products.Count());
    }

    [Fact]
    public async Task ShouldRejectDuplicateProductCode()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();

        var service = CreateProductService(context);
        var request = CreateProductRequest("SNK-WHT-07-001");

        await Assert.ThrowsAsync<DuplicateProductCodeException>(() => service.CreateAsync(request));
    }

    [Fact]
    public async Task ShouldFilterProductsBySizeAndColor()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();

        var service = CreateProductService(context);
        var filters = new ProductFilterRequest(null, null, null, ProductSize.Ten, ProductColor.Gray);

        var products = await service.ListAsync(filters);

        Assert.NotEmpty(products);
        Assert.All(products, product =>
        {
            Assert.Equal(ProductSize.Ten, product.Size);
            Assert.Equal(ProductColor.Gray, product.Color);
        });
    }

    private static SqliteConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        return connection;
    }

    private static EcommerceDbContext CreateContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<EcommerceDbContext>()
            .UseSqlite(connection)
            .Options;

        return new EcommerceDbContext(options);
    }

    private static ProductService CreateProductService(EcommerceDbContext context)
    {
        return new ProductService(new ProductRepository(context));
    }

    private static CreateProductRequest CreateProductRequest(string code)
    {
        return new CreateProductRequest(
            code,
            "https://placehold.co/600x400?text=New+Sneaker",
            "New Sneaker",
            "New sneaker for service test.",
            ProductSize.Seven,
            ProductColor.White,
            199900m,
            5);
    }
}
