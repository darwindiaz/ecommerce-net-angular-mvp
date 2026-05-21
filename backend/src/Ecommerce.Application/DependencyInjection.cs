using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Application.Interfaces.Cart;
using Ecommerce.Application.Interfaces.Orders;
using Ecommerce.Application.Interfaces.Products;
using Ecommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
