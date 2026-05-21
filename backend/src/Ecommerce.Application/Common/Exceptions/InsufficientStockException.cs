namespace Ecommerce.Application.Common.Exceptions;

public class InsufficientStockException(string productCode, int requestedQuantity, int availableStock)
    : Exception($"Product '{productCode}' has only {availableStock} units available. Requested quantity: {requestedQuantity}.");
