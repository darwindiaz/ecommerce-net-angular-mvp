namespace Ecommerce.Application.DTOs.Auth;

public record TokenResult(
    string Token,
    DateTime ExpiresAt);
