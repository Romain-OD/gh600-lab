using Payments;
using Xunit;

namespace Payments.Tests;

public class PaymentProcessorTests
{
    private static PaymentRequest Request(decimal amount = 25m) => new()
    {
        customer_ref = "cus_1",
        Amount = amount,
        Currency = "EUR",
        card_token = "tok_visa",
    };

    [Fact]
    public void Process_ValidRequest_Captures()
    {
        var processor = new PaymentProcessor(new StripeGateway());

        var result = processor.Process(Request());

        Assert.True(result.Success);
        Assert.Equal(PaymentStatus.Captured, processor.Find(result.PaymentId!)!.Status);
    }

    [Fact]
    public void Process_MissingCardToken_Fails()
    {
        var processor = new PaymentProcessor(new StripeGateway());

        var result = processor.Process(new PaymentRequest
        {
            customer_ref = "cus_1",
            Amount = 25m,
            Currency = "EUR",
        });

        Assert.False(result.Success);
    }
}
