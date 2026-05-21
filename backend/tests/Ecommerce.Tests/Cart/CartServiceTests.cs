using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Tests.Cart;

public class CartServiceTests
{
    [Fact]
    public async Task ShouldAddProductToCart()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync();

        var service = CreateCartService(context);
        var cart = await service.AddItemAsync(userId, new AddCartItemRequest(product.Id, 2));

        Assert.Single(cart.Items);
        Assert.Equal(2, cart.Items.Single().Quantity);
        Assert.Equal(product.Price * 2, cart.Total);
    }

    [Fact]
    public async Task ShouldIncreaseQuantityWhenAddingExistingProduct()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync();

        var service = CreateCartService(context);
        await service.AddItemAsync(userId, new AddCartItemRequest(product.Id, 1));
        var cart = await service.AddItemAsync(userId, new AddCartItemRequest(product.Id, 2));

        Assert.Single(cart.Items);
        Assert.Equal(3, cart.Items.Single().Quantity);
    }

    [Fact]
    public async Task ShouldRejectInsufficientStock()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync();

        var service = CreateCartService(context);
        var request = new AddCartItemRequest(product.Id, product.Stock + 1);

        await Assert.ThrowsAsync<InsufficientStockException>(() => service.AddItemAsync(userId, request));
    }

    [Fact]
    public async Task ShouldUpdateAndDeleteCartItem()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync(product => product.Stock >= 4);

        var service = CreateCartService(context);
        var cart = await service.AddItemAsync(userId, new AddCartItemRequest(product.Id, 2));
        var itemId = cart.Items.Single().Id;

        var updatedCart = await service.UpdateItemAsync(userId, new UpdateCartItemRequest(itemId, 4));
        await service.DeleteItemAsync(userId, itemId);
        var emptyCart = await service.GetAsync(userId);

        Assert.Equal(4, updatedCart.Items.Single().Quantity);
        Assert.Empty(emptyCart.Items);
        Assert.Equal(0, emptyCart.Total);
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

    private static CartService CreateCartService(EcommerceDbContext context)
    {
        return new CartService(
            new CartRepository(context),
            new ProductRepository(context));
    }

    private static async Task<Guid> CreateUserAsync(EcommerceDbContext context)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Names = "Carlos",
            LastNames = "Ruiz",
            Age = 31,
            BirthDate = new DateOnly(1995, 4, 10),
            Country = "Colombia",
            Department = "Antioquia",
            City = "Medellin",
            Phone = "3000000000",
            Address = "Calle 1 # 2-3",
            Email = $"cart-{Guid.NewGuid():N}@example.com",
            PasswordHash = "hash",
            Role = UserRole.Customer
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.Id;
    }
}
