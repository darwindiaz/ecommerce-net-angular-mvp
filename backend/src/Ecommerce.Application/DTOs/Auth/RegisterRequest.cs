namespace Ecommerce.Application.DTOs.Auth;

public record RegisterRequest(
    string Names,
    string LastNames,
    int Age,
    DateOnly BirthDate,
    string Country,
    string Department,
    string City,
    string Phone,
    string Address,
    string Email,
    string Password);
