namespace Ecommerce.Application.DTOs.Cart;

public record UpdateCartItemRequest(
    Guid CartItemId,
    int Quantity);
