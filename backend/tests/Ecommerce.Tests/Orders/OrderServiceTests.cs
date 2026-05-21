using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.DTOs.Orders;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Infrastructure.ExternalServices;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using DomainCart = Ecommerce.Domain.Entities.Cart;

namespace Ecommerce.Tests.Orders;

public class OrderServiceTests
{
    [Fact]
    public async Task ShouldCreateOrderFromCart()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync(product => product.Stock >= 3);
        var initialStock = product.Stock;
        await AddCartItemAsync(context, userId, product.Id, 3);

        var service = CreateOrderService(context);
        var order = await service.CreateFromCartAsync(userId);
        var updatedProduct = await context.Products.SingleAsync(product => product.Id == order.Items.Single().ProductId);
        var cart = await context.Carts
            .Include(cart => cart.Items)
            .SingleAsync(cart => cart.UserId == userId);

        Assert.Equal(OrderStatus.InProcess, order.Status);
        Assert.Equal(product.Price * 3, order.Total);
        Assert.Equal(initialStock - 3, updatedProduct.Stock);
        Assert.Empty(cart.Items);
    }

    [Fact]
    public async Task ShouldRejectEmptyCart()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);

        var service = CreateOrderService(context);

        await Assert.ThrowsAsync<EmptyCartException>(() => service.CreateFromCartAsync(userId));
    }

    [Fact]
    public async Task ShouldUpdateOrderStatus()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var userId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync(product => product.Stock >= 1);
        await AddCartItemAsync(context, userId, product.Id, 1);

        var service = CreateOrderService(context);
        var order = await service.CreateFromCartAsync(userId);
        var updatedOrder = await service.UpdateStatusAsync(
            order.Id,
            new UpdateOrderStatusRequest(OrderStatus.Shipped));

        Assert.Equal(OrderStatus.Shipped, updatedOrder.Status);
    }

    [Fact]
    public async Task ShouldListOnlyCustomerOrdersForCustomer()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();
        var firstUserId = await CreateUserAsync(context);
        var secondUserId = await CreateUserAsync(context);
        var product = await context.Products.FirstAsync(product => product.Stock >= 2);
        await AddCartItemAsync(context, firstUserId, product.Id, 1);
        await AddCartItemAsync(context, secondUserId, product.Id, 1);

        var service = CreateOrderService(context);
        await service.CreateFromCartAsync(firstUserId);
        await service.CreateFromCartAsync(secondUserId);

        var customerOrders = await service.ListAsync(
            firstUserId,
            isAdmin: false,
            new OrderFilterRequest(null));

        Assert.Single(customerOrders);
        Assert.Equal(firstUserId, customerOrders.Single().UserId);
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

    private static OrderService CreateOrderService(EcommerceDbContext context)
    {
        return new OrderService(
            new CartRepository(context),
            new OrderRepository(context),
            new CashOnDeliveryProvider());
    }

    private static async Task<Guid> CreateUserAsync(EcommerceDbContext context)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Names = "Laura",
            LastNames = "Rios",
            Age = 30,
            BirthDate = new DateOnly(1996, 2, 15),
            Country = "Colombia",
            Department = "Antioquia",
            City = "Medellin",
            Phone = "3000000000",
            Address = "Calle 20 # 30-40",
            Email = $"order-{Guid.NewGuid():N}@example.com",
            PasswordHash = "hash",
            Role = UserRole.Customer
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user.Id;
    }

    private static async Task AddCartItemAsync(
        EcommerceDbContext context,
        Guid userId,
        Guid productId,
        int quantity)
    {
        var cart = new DomainCart
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };

        cart.Items.Add(new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cart.Id,
            ProductId = productId,
            Quantity = quantity
        });

        context.Carts.Add(cart);
        await context.SaveChangesAsync();
    }
}
