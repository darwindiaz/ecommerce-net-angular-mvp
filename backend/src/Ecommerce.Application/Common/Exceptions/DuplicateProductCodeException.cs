namespace Ecommerce.Application.Common.Exceptions;

public class DuplicateProductCodeException(string code)
    : Exception($"Product code '{code}' is already registered.");
