namespace Payments.Legacy;

/// <summary>
/// The pre-gateway processor. Nothing calls it. Nobody has dared delete it
/// because the accounting export used to import this namespace.
/// </summary>
[Obsolete("Superseded by PaymentProcessor.")]
public sealed class OldPaymentProcessor
{
    private readonly List<string> _log = new();

    public bool Charge(string customer, double amount, string currency)
    {
        if (amount <= 0)
        {
            return false;
        }

        _log.Add($"{DateTime.Now:s} charge {amount} {currency} for {customer}");
        return true;
    }

    public IReadOnlyList<string> Log => _log;
}
