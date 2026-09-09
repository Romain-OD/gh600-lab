using Payments;
using Payments.Validation;
using Xunit;

namespace Payments.Tests;

public class RequestValidatorTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(10001)]
    public void Validate_AmountOutsideTheLimits_IsRejected(decimal amount)
    {
        var problem = new RequestValidator().Validate(new PaymentRequest
        {
            customer_ref = "cus_1",
            Amount = amount,
            Currency = "EUR",
        });

        Assert.NotNull(problem);
    }

    [Fact]
    public void Validate_GoodRequest_IsAccepted()
    {
        var problem = new RequestValidator().Validate(new PaymentRequest
        {
            customer_ref = "cus_1",
            Amount = 25m,
            Currency = "EUR",
        });

        Assert.Null(problem);
    }
}
