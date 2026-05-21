using Ecommerce.Application.DTOs.Products;

namespace Ecommerce.Application.Interfaces.Products;

public interface IProductService
{
    Task<IReadOnlyCollection<ProductResponse>> ListAsync(
        ProductFilterRequest filters,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductResponse> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<ProductResponse> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
