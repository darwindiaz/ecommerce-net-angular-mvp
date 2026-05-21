using System.Security.Claims;
using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Cart;
using Ecommerce.Application.Interfaces.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CartResponse>> Get(CancellationToken cancellationToken)
    {
        var cart = await cartService.GetAsync(GetCurrentUserId(), cancellationToken);

        return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<ActionResult<CartResponse>> AddItem(
        AddCartItemRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cart = await cartService.AddItemAsync(GetCurrentUserId(), request, cancellationToken);
            return Ok(cart);
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InsufficientStockException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("items")]
    public async Task<ActionResult<CartResponse>> UpdateItem(
        UpdateCartItemRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cart = await cartService.UpdateItemAsync(GetCurrentUserId(), request, cancellationToken);
            return Ok(cart);
        }
        catch (CartItemNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (InsufficientStockException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpDelete("items/{id:guid}")]
    public async Task<IActionResult> DeleteItem(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await cartService.DeleteItemAsync(GetCurrentUserId(), id, cancellationToken);
            return NoContent();
        }
        catch (CartItemNotFoundException exception)
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
