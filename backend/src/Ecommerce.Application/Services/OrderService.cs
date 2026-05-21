using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Orders;
using Ecommerce.Application.DTOs.Payments;
using Ecommerce.Application.Interfaces.Orders;
using Ecommerce.Application.Interfaces.Payments;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Services;

public class OrderService(
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    IPaymentProvider paymentProvider) : IOrderService
{
    private const string CashOnDelivery = "CashOnDelivery";

    public async Task<OrderResponse> CreateFromCartAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var cart = await cartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken)
            ?? throw new EmptyCartException(userId);

        if (cart.Items.Count == 0)
        {
            throw new EmptyCartException(userId);
        }

        foreach (var item in cart.Items)
        {
            if (item.Product is null)
            {
                throw new ProductNotFoundException(item.ProductId);
            }

            if (item.Quantity > item.Product.Stock)
            {
                throw new InsufficientStockException(item.Product.Code, item.Quantity, item.Product.Stock);
            }
        }

        var total = cart.Items.Sum(item => item.Quantity * item.Product!.Price);
        var paymentResult = await paymentProvider.ProcessAsync(
            new PaymentRequest(userId, total, CashOnDelivery),
            cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            throw new InvalidOperationException(paymentResult.Message);
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Total = total,
            Status = OrderStatus.InProcess,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var cartItem in cart.Items)
        {
            var product = cartItem.Product!;
            product.Stock -= cartItem.Quantity;

            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = product.Id,
                Product = product,
                Price = product.Price,
                Quantity = cartItem.Quantity
            });
        }

        await orderRepository.AddAsync(order, cancellationToken);
        cartRepository.RemoveItems(cart.Items);
        await orderRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(order);
    }

    public async Task<IReadOnlyCollection<OrderResponse>> ListAsync(
        Guid userId,
        bool isAdmin,
        OrderFilterRequest filters,
        CancellationToken cancellationToken = default)
    {
        var orders = await orderRepository.ListAsync(
            isAdmin ? null : userId,
            filters,
            cancellationToken);

        return orders.Select(ToResponse).ToList();
    }

    public async Task<OrderResponse> UpdateStatusAsync(
        Guid orderId,
        UpdateOrderStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(orderId);

        order.Status = request.Status;
        await orderRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(order);
    }

    public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(orderId);

        orderRepository.Delete(order);
        await orderRepository.SaveChangesAsync(cancellationToken);
    }

    private static OrderResponse ToResponse(Order order)
    {
        var items = order.Items
            .OrderBy(item => item.Product?.Name)
            .Select(item =>
            {
                var product = item.Product
                    ?? throw new InvalidOperationException($"Product '{item.ProductId}' was not loaded.");

                return new OrderItemResponse(
                    item.Id,
                    item.ProductId,
                    product.Code,
                    product.Name,
                    item.Price,
                    item.Quantity,
                    item.Price * item.Quantity);
            })
            .ToList();

        return new OrderResponse(
            order.Id,
            order.UserId,
            order.Total,
            order.Status,
            order.CreatedAt,
            items);
    }
}
