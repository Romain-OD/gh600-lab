namespace Payments;

/// <summary>An amount and its currency.</summary>
public readonly struct Money
{
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public string Currency { get; }

    // Used by the legacy mapper. Nobody has decided whether this should exist.
    public static Money FromDouble(double amount, string currency) =>
        new((decimal)amount, currency);

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount, a.Currency);

    public static Money operator -(Money a, Money b) => new(a.Amount - b.Amount, a.Currency);

    public override string ToString() => $"{Amount:0.00} {Currency}";
}
