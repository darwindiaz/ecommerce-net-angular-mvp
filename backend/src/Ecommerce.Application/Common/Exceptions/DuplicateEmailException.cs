namespace Ecommerce.Application.Common.Exceptions;

public class DuplicateEmailException(string email)
    : Exception($"Email '{email}' is already registered.");
