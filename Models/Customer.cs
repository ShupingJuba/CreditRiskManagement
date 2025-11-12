using System.Text.Json.Serialization;

namespace CreditRiskManagement.Models
{
    /// <summary>
    /// Represents a customer with credit information for risk assessment.
    /// </summary>
    public class Customer
    {
        [JsonPropertyName("CustomerId")]
        public int CustomerId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("PaymentHistory")]
        public double PaymentHistory { get; set; }

        [JsonPropertyName("CreditUtilization")]
        public double CreditUtilization { get; set; }

        [JsonPropertyName("AgeOfCreditHistory")]
        public double AgeOfCreditHistory { get; set; }

        /// <summary>
        /// Validates that all customer properties are within acceptable ranges.
        /// </summary>
        /// <returns>True if valid, false otherwise.</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name)
                   && PaymentHistory >= 0 && PaymentHistory <= 100
                   && CreditUtilization >= 0 && CreditUtilization <= 100
                   && AgeOfCreditHistory >= 0;
        }
    }
}
