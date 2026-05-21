namespace Ecommerce.Application.Common.Exceptions;

public class CartItemNotFoundException(Guid cartItemId)
    : Exception($"Cart item '{cartItemId}' was not found.");
