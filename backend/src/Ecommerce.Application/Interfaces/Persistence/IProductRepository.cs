using Ecommerce.Application.DTOs.Products;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Persistence;

public interface IProductRepository
{
    Task<IReadOnlyCollection<Product>> ListAsync(
        ProductFilterRequest filters,
        CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, Guid? excludedProductId = null, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    void Delete(Product product);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
