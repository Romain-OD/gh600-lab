namespace Payments;

// Same shape as StripeGateway, copied and edited. Somebody meant to come back to this.
public sealed class PaypalGateway : IPayGw
{
    private readonly Dictionary<string, Money> _authorized = new();

    public string Name => "paypal";

    public string Authorize(Money amount, string cardToken)
    {
        if (string.IsNullOrWhiteSpace(cardToken))
        {
            throw new ArgumentException("card token is required", nameof(cardToken));
        }

        var reference = "PAY-" + Guid.NewGuid().ToString("N")[..16].ToUpperInvariant();
        _authorized[reference] = amount;
        return reference;
    }

    public void Capture(string gatewayReference)
    {
        if (!_authorized.ContainsKey(gatewayReference))
        {
            throw new InvalidOperationException("unknown authorization " + gatewayReference);
        }
    }

    public void RefundAmount(string gatewayReference, Money amount)
    {
        if (!_authorized.TryGetValue(gatewayReference, out var authorized))
        {
            throw new InvalidOperationException("unknown authorization " + gatewayReference);
        }

        if (amount.Amount > authorized.Amount)
        {
            throw new InvalidOperationException("refund exceeds authorization");
        }
    }
}
