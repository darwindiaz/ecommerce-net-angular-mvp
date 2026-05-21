namespace Ecommerce.Application.DTOs.Orders;

public record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductCode,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);
