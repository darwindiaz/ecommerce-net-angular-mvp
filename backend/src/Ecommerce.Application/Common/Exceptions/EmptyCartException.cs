namespace Ecommerce.Application.Common.Exceptions;

public class EmptyCartException(Guid userId)
    : Exception($"User '{userId}' does not have items in the cart.");
