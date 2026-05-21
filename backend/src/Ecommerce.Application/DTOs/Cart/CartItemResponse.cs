namespace Ecommerce.Application.DTOs.Cart;

public record CartItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductCode,
    string ProductName,
    string ImageUrl,
    decimal UnitPrice,
    int Quantity,
    decimal Subtotal);
