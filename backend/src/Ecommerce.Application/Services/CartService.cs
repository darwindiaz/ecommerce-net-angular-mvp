using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.Interfaces.Cart;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Services;

public class CartService(
    ICartRepository cartRepository,
    IProductRepository productRepository) : ICartService
{
    public async Task<CartResponse> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await GetOrCreateCartAsync(userId, cancellationToken);

        return ToResponse(cart);
    }

    public async Task<CartResponse> AddItemAsync(
        Guid userId,
        AddCartItemRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateQuantity(request.Quantity);

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new ProductNotFoundException(request.ProductId);

        var cart = await GetOrCreateCartAsync(userId, cancellationToken);
        var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == request.ProductId);
        var newQuantity = request.Quantity + (existingItem?.Quantity ?? 0);
        var isNewItem = existingItem is null;

        EnsureStock(product, newQuantity);

        if (isNewItem)
        {
            existingItem = new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = request.Quantity
            };

            await cartRepository.AddItemAsync(existingItem, cancellationToken);
        }
        else
        {
            existingItem!.Quantity = newQuantity;
        }

        await cartRepository.SaveChangesAsync(cancellationToken);
        existingItem!.Product = product;

        return ToResponse(cart);
    }

    public async Task<CartResponse> UpdateItemAsync(
        Guid userId,
        UpdateCartItemRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateQuantity(request.Quantity);

        var item = await cartRepository.GetItemAsync(userId, request.CartItemId, cancellationToken)
            ?? throw new CartItemNotFoundException(request.CartItemId);

        if (item.Product is null)
        {
            throw new ProductNotFoundException(item.ProductId);
        }

        EnsureStock(item.Product, request.Quantity);
        item.Quantity = request.Quantity;

        await cartRepository.SaveChangesAsync(cancellationToken);

        var cart = await cartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken)
            ?? throw new InvalidOperationException("Cart was not found after updating an item.");

        return ToResponse(cart);
    }

    public async Task DeleteItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default)
    {
        var item = await cartRepository.GetItemAsync(userId, cartItemId, cancellationToken)
            ?? throw new CartItemNotFoundException(cartItemId);

        cartRepository.RemoveItem(item);
        await cartRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Cart> GetOrCreateCartAsync(Guid userId, CancellationToken cancellationToken)
    {
        var cart = await cartRepository.GetByUserIdWithItemsAsync(userId, cancellationToken);

        if (cart is not null)
        {
            return cart;
        }

        cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = userId
        };

        await cartRepository.AddAsync(cart, cancellationToken);
        await cartRepository.SaveChangesAsync(cancellationToken);

        return cart;
    }

    private static CartResponse ToResponse(Cart cart)
    {
        var items = cart.Items
            .OrderBy(item => item.Product?.Name)
            .Select(item =>
            {
                var product = item.Product
                    ?? throw new InvalidOperationException($"Product '{item.ProductId}' was not loaded.");
                var subtotal = product.Price * item.Quantity;

                return new CartItemResponse(
                    item.Id,
                    product.Id,
                    product.Code,
                    product.Name,
                    product.ImageUrl,
                    product.Price,
                    item.Quantity,
                    subtotal);
            })
            .ToList();

        return new CartResponse(
            cart.Id,
            cart.UserId,
            items,
            items.Sum(item => item.Subtotal));
    }

    private static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.");
        }
    }

    private static void EnsureStock(Product product, int quantity)
    {
        if (quantity > product.Stock)
        {
            throw new InsufficientStockException(product.Code, quantity, product.Stock);
        }
    }
}
