namespace Ecommerce.Application.DTOs.Auth;

public record AuthResponse(
    Guid UserId,
    string Email,
    string Role,
    string Token,
    DateTime ExpiresAt);
