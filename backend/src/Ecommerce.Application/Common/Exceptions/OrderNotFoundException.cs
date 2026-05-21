namespace Ecommerce.Application.Common.Exceptions;

public class OrderNotFoundException(Guid orderId)
    : Exception($"Order '{orderId}' was not found.");
