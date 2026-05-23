using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Infrastructure.Persistence;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var isEnabled = bool.TryParse(configuration["DemoAdmin:Enabled"], out var enabled) && enabled;

        if (!isEnabled)
        {
            return;
        }

        var dbContext = scope.ServiceProvider.GetRequiredService<EcommerceDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var email = configuration["DemoAdmin:Email"]?.Trim().ToLowerInvariant()
            ?? throw new InvalidOperationException("Demo admin email was not configured.");
        var password = configuration["DemoAdmin:Password"]
            ?? throw new InvalidOperationException("Demo admin password was not configured.");

        await dbContext.Database.MigrateAsync(cancellationToken);

        var admin = await dbContext.Users.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (admin is null)
        {
            var birthDate = new DateOnly(1990, 1, 1);
            dbContext.Users.Add(new User
            {
                Id = Guid.Parse("20000000-0000-0000-0000-000000000001"),
                Names = "Admin",
                LastNames = "Demo",
                Age = CalculateAge(birthDate),
                BirthDate = birthDate,
                Country = "Colombia",
                Department = "Demo",
                City = "Demo",
                Phone = "3000000000",
                Address = "Demo address",
                Email = email,
                PasswordHash = passwordHasher.Hash(password),
                Role = UserRole.Admin
            });
        }
        else
        {
            admin.Role = UserRole.Admin;

            if (!passwordHasher.Verify(password, admin.PasswordHash))
            {
                admin.PasswordHash = passwordHasher.Hash(password);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - birthDate.Year;

        return birthDate > today.AddYears(-age) ? age - 1 : age;
    }
}
