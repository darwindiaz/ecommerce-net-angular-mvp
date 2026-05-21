namespace Ecommerce.Application.DTOs.Cart;

public record CartResponse(
    Guid Id,
    Guid UserId,
    IReadOnlyCollection<CartItemResponse> Items,
    decimal Total);
