using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.Products;

public record UpdateProductRequest(
    string Code,
    string ImageUrl,
    string Name,
    string Description,
    ProductSize Size,
    ProductColor Color,
    decimal Price,
    int Stock);
