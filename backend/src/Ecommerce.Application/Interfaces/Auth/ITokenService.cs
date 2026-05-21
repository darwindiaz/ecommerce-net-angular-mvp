using Ecommerce.Application.DTOs.Auth;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Auth;

public interface ITokenService
{
    TokenResult Generate(User user);
}
