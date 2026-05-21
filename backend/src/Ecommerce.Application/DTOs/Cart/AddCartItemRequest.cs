namespace Ecommerce.Application.DTOs.Cart;

public record AddCartItemRequest(
    Guid ProductId,
    int Quantity);
