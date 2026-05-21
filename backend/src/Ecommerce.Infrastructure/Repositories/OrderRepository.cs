using Ecommerce.Application.DTOs.Orders;
using Ecommerce.Application.Interfaces.Persistence;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Repositories;

public class OrderRepository(EcommerceDbContext dbContext) : IOrderRepository
{
    public async Task<IReadOnlyCollection<Order>> ListAsync(
        Guid? userId,
        OrderFilterRequest filters,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Orders
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(order => order.UserId == userId.Value);
        }

        if (filters.Status.HasValue)
        {
            query = query.Where(order => order.Status == filters.Status.Value);
        }

        return await query
            .OrderByDescending(order => order.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return dbContext.Orders
            .Include(order => order.Items)
            .ThenInclude(item => item.Product)
            .FirstOrDefaultAsync(order => order.Id == orderId, cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await dbContext.Orders.AddAsync(order, cancellationToken);
    }

    public void Delete(Order order)
    {
        dbContext.Orders.Remove(order);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
