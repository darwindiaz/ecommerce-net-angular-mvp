namespace Ecommerce.Application.DTOs.Payments;

public record PaymentRequest(
    Guid UserId,
    decimal Amount,
    string Method);
