using Payments;
using Xunit;

namespace Payments.Tests;

public class RefundServiceTests
{
    private static (Payment payment, RefundService refunds) Captured(decimal amount)
    {
        var gateway = new StripeGateway();
        var processor = new PaymentProcessor(gateway);
        var result = processor.Process(new PaymentRequest
        {
            customer_ref = "cus_1",
            Amount = amount,
            Currency = "EUR",
            card_token = "tok_visa",
        });

        var payment = processor.Find(result.PaymentId!)!;
        return (payment, new RefundService(gateway));
    }

    [Fact]
    public void Refund_FullAmount_Succeeds()
    {
        var (payment, refunds) = Captured(50m);

        var result = refunds.Refund(payment, new Money(50m, "EUR"));

        Assert.True(result.Success);
        Assert.Equal(PaymentStatus.Refunded, payment.Status);
        Assert.Equal(50m, payment.RefundedTotal.Amount);
    }

    [Fact]
    public void Refund_MoreThanThePayment_IsRejected()
    {
        var (payment, refunds) = Captured(50m);

        var result = refunds.Refund(payment, new Money(60m, "EUR"));

        Assert.False(result.Success);
    }

    [Fact]
    public void Refund_WrongCurrency_IsRejected()
    {
        var (payment, refunds) = Captured(50m);

        var result = refunds.Refund(payment, new Money(10m, "USD"));

        Assert.False(result.Success);
    }

    // Two partial refunds of 30 give the customer 60 back on a 50 payment.
    // Each one is checked against the total, so the second one goes through.
    [Fact]
    public void Refund_PartialRefunds_CannotExceedThePaymentInTotal()
    {
        var (payment, refunds) = Captured(50m);

        var first = refunds.Refund(payment, new Money(30m, "EUR"));
        var second = refunds.Refund(payment, new Money(30m, "EUR"));

        Assert.True(first.Success);
        Assert.False(second.Success);
        Assert.Equal(30m, payment.RefundedTotal.Amount);
    }
}
