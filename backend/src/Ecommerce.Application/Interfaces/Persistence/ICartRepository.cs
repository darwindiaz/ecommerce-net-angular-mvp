using Ecommerce.Domain.Entities;
using DomainCart = Ecommerce.Domain.Entities.Cart;

namespace Ecommerce.Application.Interfaces.Persistence;

public interface ICartRepository
{
    Task<DomainCart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<DomainCart?> GetByUserIdWithItemsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default);
    Task AddAsync(DomainCart cart, CancellationToken cancellationToken = default);
    Task AddItemAsync(CartItem item, CancellationToken cancellationToken = default);
    void RemoveItem(CartItem item);
    void RemoveItems(IEnumerable<CartItem> items);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
