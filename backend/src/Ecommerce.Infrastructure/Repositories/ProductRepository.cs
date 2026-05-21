using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class ProductRepository(EcommerceDbContext dbContext) : IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> ListAsync(
        ProductFilterRequest filters,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filters.Name))
        {
            var name = filters.Name.Trim();
            query = query.Where(product => product.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(filters.Description))
        {
            var description = filters.Description.Trim();
            query = query.Where(product => product.Description.Contains(description));
        }

        if (!string.IsNullOrWhiteSpace(filters.Code))
        {
            var code = filters.Code.Trim();
            query = query.Where(product => product.Code.Contains(code));
        }

        if (filters.Size.HasValue)
        {
            query = query.Where(product => product.Size == filters.Size.Value);
        }

        if (filters.Color.HasValue)
        {
            query = query.Where(product => product.Color == filters.Color.Value);
        }

        return await query
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(
        string code,
        Guid? excludedProductId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Products.AnyAsync(
            product => product.Code == code && (!excludedProductId.HasValue || product.Id != excludedProductId.Value),
            cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await dbContext.Products.AddAsync(product, cancellationToken);
    }

    public void Delete(Product product)
    {
        dbContext.Products.Remove(product);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
