namespace Payments.Validation;

public sealed class RequestValidator
{
    public string? Validate(PaymentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.customer_ref))
        {
            return "customer reference is required";
        }

        if (request.customer_ref.Length > 64)
        {
            return "customer reference is too long";
        }

        if (request.Amount <= 0m)
        {
            return "amount must be positive";
        }

        if (request.Amount > 10000m)
        {
            return "amount is above the per-transaction limit";
        }

        if (request.Currency.Length != 3)
        {
            return "currency must be a 3 letter code";
        }

        return null;
    }
}
