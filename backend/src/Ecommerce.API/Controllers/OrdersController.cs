using System.Security.Claims;
using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Orders;
using Ecommerce.Application.Interfaces.Orders;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderService.CreateFromCartAsync(GetCurrentUserId(), cancellationToken);
            return CreatedAtAction(nameof(List), new { id = order.Id }, order);
        }
        catch (EmptyCartException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
        catch (InsufficientStockException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<OrderResponse>>> List(
        [FromQuery] OrderStatus? status,
        CancellationToken cancellationToken)
    {
        var orders = await orderService.ListAsync(
            GetCurrentUserId(),
            User.IsInRole(nameof(UserRole.Admin)),
            new OrderFilterRequest(status),
            cancellationToken);

        return Ok(orders);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(
        Guid id,
        UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderService.UpdateStatusAsync(id, request, cancellationToken);
            return Ok(order);
        }
        catch (OrderNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await orderService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (OrderNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            throw new UnauthorizedAccessException("Authenticated user id claim is missing or invalid.");
        }

        return parsedUserId;
    }
}
