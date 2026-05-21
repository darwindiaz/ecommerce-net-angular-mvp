using Ecommerce.Application.DTOs.Orders;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Persistence;

public interface IOrderRepository
{
    Task<IReadOnlyCollection<Order>> ListAsync(
        Guid? userId,
        OrderFilterRequest filters,
        CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    void Delete(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
