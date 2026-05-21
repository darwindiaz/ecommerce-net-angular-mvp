using Ecommerce.Application.DTOs.Payments;

namespace Ecommerce.Application.Interfaces.Payments;

public interface IPaymentProvider
{
    Task<PaymentResult> ProcessAsync(PaymentRequest request, CancellationToken cancellationToken = default);
}
