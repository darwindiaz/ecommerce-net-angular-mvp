using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.Products;

public record ProductFilterRequest(
    string? Name,
    string? Description,
    string? Code,
    ProductSize? Size,
    ProductColor? Color);
