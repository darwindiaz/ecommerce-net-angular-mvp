using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.DTOs.Orders;

public record OrderFilterRequest(OrderStatus? Status);
