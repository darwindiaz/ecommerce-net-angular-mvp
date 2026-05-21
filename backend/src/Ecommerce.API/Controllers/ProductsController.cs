using Ecommerce.Application.Common.Exceptions;
using Ecommerce.Application.DTOs.Products;
using Ecommerce.Application.Interfaces.Products;
using Ecommerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<ProductResponse>>> List(
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? code,
        [FromQuery] ProductSize? size,
        [FromQuery] ProductColor? color,
        CancellationToken cancellationToken)
    {
        var filters = new ProductFilterRequest(name, description, code, size, color);
        var products = await productService.ListAsync(filters, cancellationToken);

        return Ok(products);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productService.GetByIdAsync(id, cancellationToken);
            return Ok(product);
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(
        CreateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        catch (DuplicateProductCodeException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProductResponse>> Update(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = await productService.UpdateAsync(id, request, cancellationToken);
            return Ok(product);
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
        catch (DuplicateProductCodeException exception)
        {
            return Conflict(new { message = exception.Message });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await productService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (ProductNotFoundException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
