using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public required string Names { get; set; }
    public required string LastNames { get; set; }
    public int Age { get; set; }
    public DateOnly BirthDate { get; set; }
    public required string Country { get; set; }
    public required string Department { get; set; }
    public required string City { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public UserRole Role { get; set; }

    public Cart? Cart { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
}
