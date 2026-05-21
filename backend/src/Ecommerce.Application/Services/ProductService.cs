using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Application.Interfaces.Products;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

public class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IReadOnlyCollection<ProductResponse>> ListAsync(
        ProductFilterRequest filters,
        CancellationToken cancellationToken = default)
    {
        var products = await productRepository.ListAsync(filters, cancellationToken);

        return products.Select(ToResponse).ToList();
    }

    public async Task<ProductResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);

        return ToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateProduct(request.Code, request.ImageUrl, request.Name, request.Description, request.Price, request.Stock);

        var code = NormalizeCode(request.Code);
        if (await productRepository.ExistsByCodeAsync(code, cancellationToken: cancellationToken))
        {
            throw new DuplicateProductCodeException(code);
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Code = code,
            ImageUrl = request.ImageUrl.Trim(),
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            Size = request.Size,
            Color = request.Color,
            Price = request.Price,
            Stock = request.Stock
        };

        await productRepository.AddAsync(product, cancellationToken);
        await productRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task<ProductResponse> UpdateAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateProduct(request.Code, request.ImageUrl, request.Name, request.Description, request.Price, request.Stock);

        var product = await GetProductOrThrowAsync(id, cancellationToken);
        var code = NormalizeCode(request.Code);

        if (await productRepository.ExistsByCodeAsync(code, id, cancellationToken))
        {
            throw new DuplicateProductCodeException(code);
        }

        product.Code = code;
        product.ImageUrl = request.ImageUrl.Trim();
        product.Name = request.Name.Trim();
        product.Description = request.Description.Trim();
        product.Size = request.Size;
        product.Color = request.Color;
        product.Price = request.Price;
        product.Stock = request.Stock;

        await productRepository.SaveChangesAsync(cancellationToken);

        return ToResponse(product);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await GetProductOrThrowAsync(id, cancellationToken);

        productRepository.Delete(product);
        await productRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Product> GetProductOrThrowAsync(Guid id, CancellationToken cancellationToken)
    {
        return await productRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ProductNotFoundException(id);
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Code,
            product.ImageUrl,
            product.Name,
            product.Description,
            product.Size,
            product.Color,
            product.Price,
            product.Stock,
            product.Stock > 0);
    }

    private static void ValidateProduct(
        string code,
        string imageUrl,
        string name,
        string description,
        decimal price,
        int stock)
    {
        if (string.IsNullOrWhiteSpace(code) ||
            string.IsNullOrWhiteSpace(imageUrl) ||
            string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Product code, image, name and description are required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.");
        }

        if (stock < 0)
        {
            throw new ArgumentException("Product stock cannot be negative.");
        }
    }

    private static string NormalizeCode(string code)
    {
        return code.Trim().ToUpperInvariant();
    }
}
