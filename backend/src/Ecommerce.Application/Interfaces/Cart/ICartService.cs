using Ecommerce.Application.DTOs.Cart;

namespace Ecommerce.Application.Interfaces.Cart;

public interface ICartService
{
    Task<CartResponse> GetAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CartResponse> AddItemAsync(Guid userId, AddCartItemRequest request, CancellationToken cancellationToken = default);
    Task<CartResponse> UpdateItemAsync(Guid userId, UpdateCartItemRequest request, CancellationToken cancellationToken = default);
    Task DeleteItemAsync(Guid userId, Guid cartItemId, CancellationToken cancellationToken = default);
}
