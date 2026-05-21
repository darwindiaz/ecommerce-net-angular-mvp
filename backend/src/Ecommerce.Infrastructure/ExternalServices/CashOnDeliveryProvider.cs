using Ecommerce.Application.DTOs.Payments;
using Ecommerce.Application.Interfaces.Payments;

namespace Ecommerce.Infrastructure.ExternalServices;

public class CashOnDeliveryProvider : IPaymentProvider
{
    public Task<PaymentResult> ProcessAsync(
        PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = PaymentResult.Success(
            request.Method,
            "Cash on delivery payment registered.");

        return Task.FromResult(result);
    }
}
