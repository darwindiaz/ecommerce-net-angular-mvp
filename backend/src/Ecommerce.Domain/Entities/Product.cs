using Ecommerce.Domain.Enums;

namespace Ecommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public required string Code { get; set; }
    public required string ImageUrl { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public ProductSize Size { get; set; }
    public ProductColor Color { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
