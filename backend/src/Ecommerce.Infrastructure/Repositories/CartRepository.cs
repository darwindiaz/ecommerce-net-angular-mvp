using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class CartRepository(EcommerceDbContext dbContext) : ICartRepository
{
    public Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Carts.FirstOrDefaultAsync(cart => cart.UserId == userId, cancellationToken);
    }

    public Task<Cart?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Carts
            .Include(cart => cart.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(cart => cart.UserId == userId, cancellationToken);
    }

    public Task<CartItem?> GetItemAsync(
        Guid userId,
        Guid cartItemId,
        CancellationToken cancellationToken = default)
    {
        return dbContext.CartItems
            .Include(item => item.Cart)
            .Include(item => item.Product)
            .FirstOrDefaultAsync(
                item => item.Id == cartItemId && item.Cart != null && item.Cart.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        await dbContext.Carts.AddAsync(cart, cancellationToken);
    }

    public async Task AddItemAsync(CartItem item, CancellationToken cancellationToken = default)
    {
        await dbContext.CartItems.AddAsync(item, cancellationToken);
    }

    public void RemoveItem(CartItem item)
    {
        dbContext.CartItems.Remove(item);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
