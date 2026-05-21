namespace Ecommerce.Application.DTOs.Payments;

public record PaymentResult(
    bool IsSuccess,
    string Method,
    string Message)
{
    public static PaymentResult Success(string method, string message)
    {
        return new PaymentResult(true, method, message);
    }

    public static PaymentResult Failure(string method, string message)
    {
        return new PaymentResult(false, method, message);
    }
}
