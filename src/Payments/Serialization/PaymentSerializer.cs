using System.Text;

namespace Payments.Serialization;

/// <summary>
/// Writes a payment as JSON by hand. Predates anyone here knowing about
/// System.Text.Json, and has quietly worked ever since.
/// </summary>
public static class PaymentSerializer
{
    public static string ToJson(Payment payment)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        builder.Append("\"id\":\"").Append(Escape(payment.Id)).Append("\",");
        builder.Append("\"amount\":").Append(payment.Total.Amount.ToString("0.00")).Append(',');
        builder.Append("\"currency\":\"").Append(Escape(payment.Total.Currency)).Append("\",");
        builder.Append("\"status\":\"").Append(payment.Status).Append("\",");
        builder.Append("\"customer\":\"").Append(Escape(payment.CustomerRef)).Append('"');

        if (payment.GatewayReference is not null)
        {
            builder.Append(",\"gateway_ref\":\"").Append(Escape(payment.GatewayReference)).Append('"');
        }

        builder.Append('}');
        return builder.ToString();
    }

    private static string Escape(string value) =>
        value.Replace("\\", "\\\\").Replace("\"", "\\\"");
}
