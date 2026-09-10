namespace Payments;

public sealed class RefundService
{
    private readonly IPayGw _gateway;

    public RefundService(IPayGw gateway)
    {
        _gateway = gateway;
    }

    public PaymentResult Refund(Payment payment, Money amount)
    {
        if (payment.Status is not (PaymentStatus.Captured or PaymentStatus.Refunded))
        {
            return PaymentResult.Fail("payment is not captured");
        }

        if (amount.Amount <= 0m)
        {
            return PaymentResult.Fail("refund amount must be positive");
        }

        if (amount.Currency != payment.Total.Currency)
        {
            return PaymentResult.Fail("refund currency does not match the payment");
        }

        if (payment.RefundedTotal.Amount + amount.Amount > payment.Total.Amount)
        {
            return PaymentResult.Fail("refund exceeds the payment amount");
        }

        _gateway.RefundAmount(payment.GatewayReference!, amount);
        payment.RefundedTotal += amount;
        payment.Status = payment.RefundedTotal.Amount == payment.Total.Amount
            ? PaymentStatus.Refunded
            : PaymentStatus.Captured;
        return PaymentResult.Ok(payment.Id);
    }
}
