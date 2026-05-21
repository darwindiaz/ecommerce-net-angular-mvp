using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.Orders;

public record OrderResponse(
    Guid Id,
    Guid UserId,
    decimal Total,
    OrderStatus Status,
    DateTime CreatedAt,
    IReadOnlyCollection<OrderItemResponse> Items);
