using Ecommerce.Application.DTOs.Orders;

namespace Ecommerce.Application.Interfaces.Orders;

public interface IOrderService
{
    Task<OrderResponse> CreateFromCartAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OrderResponse>> ListAsync(
        Guid userId,
        bool isAdmin,
        OrderFilterRequest filters,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> UpdateStatusAsync(
        Guid orderId,
        UpdateOrderStatusRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
}
