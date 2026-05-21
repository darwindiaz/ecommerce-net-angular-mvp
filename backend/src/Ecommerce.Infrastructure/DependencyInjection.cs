using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Infrastructure.Authentication;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not configured.");

        services.AddDbContext<EcommerceDbContext>(options =>
            options.UseSqlite(connectionString));

        var jwtSection = configuration.GetSection(JwtSettings.SectionName);
        services.Configure<JwtSettings>(options =>
        {
            options.Issuer = jwtSection["Issuer"]
                ?? throw new InvalidOperationException("JWT issuer was not configured.");
            options.Audience = jwtSection["Audience"]
                ?? throw new InvalidOperationException("JWT audience was not configured.");
            options.SecretKey = jwtSection["SecretKey"]
                ?? throw new InvalidOperationException("JWT secret key was not configured.");
            options.ExpirationMinutes = int.TryParse(jwtSection["ExpirationMinutes"], out var expirationMinutes)
                ? expirationMinutes
                : 120;
        });
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();

        return services;
    }
}
