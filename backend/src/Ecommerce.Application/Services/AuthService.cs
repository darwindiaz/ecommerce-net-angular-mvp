using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        ValidateRegisterRequest(request);

        var email = NormalizeEmail(request.Email);
        var age = CalculateAge(request.BirthDate);

        if (await userRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new DuplicateEmailException(email);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Names = request.Names.Trim(),
            LastNames = request.LastNames.Trim(),
            Age = age,
            BirthDate = request.BirthDate,
            Country = request.Country.Trim(),
            Department = request.Department.Trim(),
            City = request.City.Trim(),
            Phone = request.Phone.Trim(),
            Address = request.Address.Trim(),
            Email = email,
            PasswordHash = passwordHasher.Hash(request.Password),
            Role = UserRole.Customer
        };

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return CreateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new InvalidCredentialsException();
        }

        var email = NormalizeEmail(request.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        return CreateAuthResponse(user);
    }

    private AuthResponse CreateAuthResponse(User user)
    {
        var tokenResult = tokenService.Generate(user);

        return new AuthResponse(
            user.Id,
            user.Email,
            user.Role.ToString(),
            tokenResult.Token,
            tokenResult.ExpiresAt);
    }

    private static void ValidateRegisterRequest(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Email is required.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            throw new ArgumentException("Password must have at least 8 characters.", nameof(request));
        }

        if (request.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("Birth date cannot be in the future.", nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Names) ||
            string.IsNullOrWhiteSpace(request.LastNames) ||
            string.IsNullOrWhiteSpace(request.Country) ||
            string.IsNullOrWhiteSpace(request.Department) ||
            string.IsNullOrWhiteSpace(request.City) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Address))
        {
            throw new ArgumentException("All user profile fields are required.", nameof(request));
        }
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;

        return birthDate > today.AddYears(-age) ? age - 1 : age;
    }
}
