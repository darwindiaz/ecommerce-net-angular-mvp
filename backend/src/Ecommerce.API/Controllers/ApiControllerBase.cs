using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            throw new UnauthorizedAccessException("Authenticated user id claim is missing or invalid.");
        }

        return parsedUserId;
    }
}
