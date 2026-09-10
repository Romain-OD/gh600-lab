namespace Payments;

/// <summary>A captured payment, and everything that has happened to it since.</summary>
public sealed class Payment
{
    public Payment(string id, Money total, string customerRef)
    {
        Id = id;
        Total = total;
        RefundedTotal = new Money(0m, total.Currency);
        CustomerRef = customerRef;
        Status = PaymentStatus.Pending;
    }

    public string Id { get; }

    public Money Total { get; }

    public Money RefundedTotal { get; internal set; }

    public string CustomerRef { get; }

    public PaymentStatus Status { get; set; }

    public string? GatewayReference { get; set; }

    public DateTimeOffset? CapturedAt { get; set; }
}
