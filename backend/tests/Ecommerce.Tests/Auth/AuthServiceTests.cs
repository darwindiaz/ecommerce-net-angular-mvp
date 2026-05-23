using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Application.Services;
using Ecommerce.Infrastructure.Authentication;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Tests.Auth;

public class AuthServiceTests
{
    [Fact]
    public async Task ShouldRegisterUserWithHashedPassword()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();

        var service = CreateAuthService(context);
        var request = CreateRegisterRequest();

        var response = await service.RegisterAsync(request);
        var user = await context.Users.SingleAsync(user => user.Email == request.Email.ToLowerInvariant());

        Assert.Equal(request.Email.ToLowerInvariant(), response.Email);
        Assert.Equal(CalculateExpectedAge(request.BirthDate), user.Age);
        Assert.NotEqual(request.Password, user.PasswordHash);
        Assert.NotEmpty(response.Token);
    }

    [Fact]
    public async Task ShouldRejectInvalidLogin()
    {
        using var connection = CreateOpenConnection();
        await using var context = CreateContext(connection);
        await context.Database.EnsureCreatedAsync();

        var service = CreateAuthService(context);
        var request = CreateRegisterRequest();
        await service.RegisterAsync(request);

        var loginRequest = new LoginRequest(request.Email, "WrongPassword123");

        await Assert.ThrowsAsync<InvalidCredentialsException>(() => service.LoginAsync(loginRequest));
    }

    private static SqliteConnection CreateOpenConnection()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        return connection;
    }

    private static EcommerceDbContext CreateContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<EcommerceDbContext>()
            .UseSqlite(connection)
            .Options;

        return new EcommerceDbContext(options);
    }

    private static AuthService CreateAuthService(EcommerceDbContext context)
    {
        return new AuthService(
            new UserRepository(context),
            new Pbkdf2PasswordHasher(),
            new TestTokenService());
    }

    private static RegisterRequest CreateRegisterRequest()
    {
        return new RegisterRequest(
            "Ana",
            "Gomez",
            99,
            new DateOnly(1998, 5, 12),
            "Colombia",
            "Antioquia",
            "Medellin",
            "3000000000",
            "Calle 10 # 20-30",
            "ana@example.com",
            "Password123*");
    }

    private static int CalculateExpectedAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;

        return birthDate > today.AddYears(-age) ? age - 1 : age;
    }

    private class TestTokenService : ITokenService
    {
        public TokenResult Generate(Domain.Entities.User user)
        {
            return new TokenResult($"test-token-{user.Id}", DateTime.UtcNow.AddHours(1));
        }
    }
}
