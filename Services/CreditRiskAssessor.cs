namespace CreditRiskManagement.Services
{
    using CreditRiskManagement.Models;

    /// <summary>
    /// Service responsible for calculating credit scores and assessing risk.
    /// </summary>
    public class CreditRiskAssessor
    {
        private const int HighRiskThreshold = 50;
        private const int MaxAgeOfCreditHistoryWeight = 10;

        /// <summary>
        /// Calculates the credit score for a customer based on their financial profile.
        /// 
        /// Formula: CreditScore = (0.4 * PaymentHistory) + (0.3 * (100 - CreditUtilization)) + (0.3 * Min(AgeOfCreditHistory, 10))
        /// 
        /// The formula weights:
        /// - Payment History (40%): Direct indicator of reliability
        /// - Credit Utilization (30%): Lower utilization is better (inverse relationship)
        /// - Age of Credit History (30%): Longer history is better, capped at 10 years
        /// </summary>
        /// <param name="paymentHistory">Percentage of payments made on time (0-100)</param>
        /// <param name="creditUtilization">Percentage of credit limit used (0-100)</param>
        /// <param name="ageOfCreditHistory">Age of credit history in years</param>
        /// <returns>Credit score as an integer (0-100)</returns>
        /// <exception cref="ArgumentException">Thrown when input values are out of valid ranges.</exception>
        public int CalculateCreditScore(double paymentHistory, double creditUtilization, double ageOfCreditHistory)
        {
            // Validate input ranges
            if (paymentHistory < 0 || paymentHistory > 100)
                throw new ArgumentException("PaymentHistory must be between 0 and 100.", nameof(paymentHistory));

            if (creditUtilization < 0 || creditUtilization > 100)
                throw new ArgumentException("CreditUtilization must be between 0 and 100.", nameof(creditUtilization));

            if (ageOfCreditHistory < 0)
                throw new ArgumentException("AgeOfCreditHistory cannot be negative.", nameof(ageOfCreditHistory));

            // Cap age of credit history at 10 years for scoring purposes
            double cappedAge = Math.Min(ageOfCreditHistory, MaxAgeOfCreditHistoryWeight);

            // Apply the credit score formula
            double score = (0.4 * paymentHistory)
                         + (0.3 * (100 - creditUtilization))
                         + (0.3 * cappedAge);

            // Round to nearest integer
            return (int)Math.Round(score);
        }

        /// <summary>
        /// Evaluates a customer's credit profile and returns a detailed result.
        /// </summary>
        /// <param name="customer">The customer to evaluate.</param>
        /// <returns>A CreditEvaluationResult with credit score and risk status.</returns>
        public CreditEvaluationResult EvaluateCustomer(Customer customer)
        {
            if (!customer.IsValid())
                throw new ArgumentException("Invalid customer data. Please check all properties are within valid ranges.");

            int creditScore = CalculateCreditScore(
                customer.PaymentHistory,
                customer.CreditUtilization,
                customer.AgeOfCreditHistory
            );

            string riskStatus = creditScore < HighRiskThreshold ? "High Risk" : "Low Risk";

            return new CreditEvaluationResult
            {
                CustomerId = customer.CustomerId,
                Name = customer.Name,
                CreditScore = creditScore,
                RiskStatus = riskStatus
            };
        }

        /// <summary>
        /// Evaluates a list of customers and returns evaluation results.
        /// </summary>
        /// <param name="customers">Collection of customers to evaluate.</param>
        /// <returns>List of CreditEvaluationResult for all customers.</returns>
        public List<CreditEvaluationResult> EvaluateCustomers(IEnumerable<Customer> customers)
        {
            return customers
                .Select(EvaluateCustomer)
                .OrderByDescending(r => r.CreditScore)
                .ToList();
        }

        /// <summary>
        /// Filters and returns only high-risk customers.
        /// </summary>
        /// <param name="results">List of evaluation results.</param>
        /// <returns>Filtered list of high-risk customers.</returns>
        public List<CreditEvaluationResult> GetHighRiskCustomers(List<CreditEvaluationResult> results)
        {
            return results.Where(r => r.RiskStatus == "High Risk").ToList();
        }
    }
}
