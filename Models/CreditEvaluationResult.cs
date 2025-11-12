namespace CreditRiskManagement.Models
{
    /// <summary>
    /// Represents the credit evaluation result for a customer.
    /// </summary>
    public class CreditEvaluationResult
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CreditScore { get; set; }
        public string RiskStatus { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"ID: {CustomerId,-4} | Name: {Name,-15} | Credit Score: {CreditScore,-3} | Risk Status: {RiskStatus}";
        }
    }
}
