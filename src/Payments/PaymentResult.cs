namespace Payments;

public sealed class PaymentResult
{
    public bool Success { get; init; }

    public string? PaymentId { get; init; }

    public string? errorMessage { get; init; }

    public static PaymentResult Ok(string paymentId) =>
        new() { Success = true, PaymentId = paymentId };

    public static PaymentResult Fail(string message) =>
        new() { Success = false, errorMessage = message };
}
