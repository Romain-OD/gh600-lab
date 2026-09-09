namespace Payments;

/// <summary>
/// A payment gateway. The name is bad and everybody knows it.
/// </summary>
public interface IPayGw
{
    string Name { get; }

    string Authorize(Money amount, string cardToken);

    void Capture(string gatewayReference);

    void RefundAmount(string gatewayReference, Money amount);
}
