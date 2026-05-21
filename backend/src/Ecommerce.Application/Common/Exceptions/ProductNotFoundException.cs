namespace Ecommerce.Application.Common.Exceptions;

public class ProductNotFoundException(Guid productId)
    : Exception($"Product '{productId}' was not found.");
