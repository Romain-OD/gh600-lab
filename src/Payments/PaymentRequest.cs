namespace Payments;

public sealed class PaymentRequest
{
    public required string customer_ref { get; init; }

    public required decimal Amount { get; init; }

    public required string Currency { get; init; }

    public string? card_token { get; init; }

    public string? IdempotencyKey { get; init; }
}
