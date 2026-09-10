using Payments.Validation;

namespace Payments;

public sealed class PaymentProcessor
{
    private readonly IPaymentGateway _gateway;
    private readonly RequestValidator _validator;
    private readonly Dictionary<string, Payment> _payments = new();

    public PaymentProcessor(IPaymentGateway gateway)
    {
        _gateway = gateway;
        _validator = new RequestValidator();
    }

    public Payment? Find(string paymentId) =>
        _payments.TryGetValue(paymentId, out var payment) ? payment : null;

    public PaymentResult Process(PaymentRequest request)
    {
        var problem = _validator.Validate(request);
        if (problem is not null)
        {
            return PaymentResult.Fail(problem);
        }

        var money = new Money(request.Amount, request.Currency);
        var payment = new Payment(Guid.NewGuid().ToString("N"), money, request.customer_ref);
        _payments[payment.Id] = payment;

        try
        {
            var reference = _gateway.Authorize(money, request.card_token ?? string.Empty);
            payment.GatewayReference = reference;
            payment.Status = PaymentStatus.Authorized;

            _gateway.Capture(reference);
            payment.Status = PaymentStatus.Captured;
            payment.CapturedAt = DateTimeOffset.UtcNow;

            return PaymentResult.Ok(payment.Id);
        }
        catch (ArgumentException ex)
        {
            payment.Status = PaymentStatus.Failed;
            return PaymentResult.Fail(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            payment.Status = PaymentStatus.Failed;
            return PaymentResult.Fail(ex.Message);
        }
    }
}
