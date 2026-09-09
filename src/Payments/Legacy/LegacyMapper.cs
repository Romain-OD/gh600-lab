namespace Payments.Legacy;

/// <summary>Maps the old flat charge rows onto the current model. Unused.</summary>
[Obsolete("Superseded by PaymentRequest.")]
public static class LegacyMapper
{
    public static Payment ToPayment(string id, double amount, string currency, string customer)
    {
        var money = Money.FromDouble(amount, currency);
        return new Payment(id, money, customer)
        {
            Status = PaymentStatus.Captured,
        };
    }

    public static string DescribeStatus(int legacyCode) => legacyCode switch
    {
        0 => "Pending",
        1 => "Authorized",
        2 => "Captured",
        3 => "Refunded",
        _ => "Failed",
    };
}
