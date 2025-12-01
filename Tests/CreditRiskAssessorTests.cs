using Xunit;
using CreditRiskManagement.Services;
using CreditRiskManagement.Models;

namespace CreditRiskManagement
{
    /// <summary>
    /// Unit tests for the CreditRiskAssessor class.
    /// Tests the CalculateCreditScore method and related functionality.
    /// </summary>
    public class CreditRiskAssessorTests
    {
        private readonly CreditRiskAssessor _assessor;

        public CreditRiskAssessorTests()
        {
            _assessor = new CreditRiskAssessor();
        }

        #region CalculateCreditScore Tests

        [Fact]
        public void CalculateCreditScore_WithExampleData_Alice_ReturnsCorrectScore()
        {
            // Arrange
            double paymentHistory = 90;
            double creditUtilization = 40;
            double ageOfCreditHistory = 5;
            // Expected: (0.4 * 90) + (0.3 * (100 - 40)) + (0.3 * 5)
            //         = 36 + 18 + 1.5 = 55.5 ≈ 56

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(56, score);
        }

        [Fact]
        public void CalculateCreditScore_WithExampleData_Bob_ReturnsCorrectScore()
        {
            // Arrange
            double paymentHistory = 70;
            double creditUtilization = 90;
            double ageOfCreditHistory = 15;
            // Expected: (0.4 * 70) + (0.3 * (100 - 90)) + (0.3 * Min(15, 10))
            //         = 28 + 3 + 3 = 34

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(34, score);
        }

        [Fact]
        public void CalculateCreditScore_WithExampleData_Charlie_ReturnsCorrectScore()
        {
            // Arrange
            double paymentHistory = 60;
            double creditUtilization = 30;
            double ageOfCreditHistory = 2;
            // Expected: (0.4 * 60) + (0.3 * (100 - 30)) + (0.3 * 2)
            //         = 24 + 21 + 0.6 = 45.6 ≈ 46

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(46, score);
        }

        [Fact]
        public void CalculateCreditScore_PerfectScenario_ReturnsMaxScore()
        {
            // Arrange: Perfect payment history, zero utilization, long credit history
            double paymentHistory = 100;
            double creditUtilization = 0;
            double ageOfCreditHistory = 20;
            // Expected: (0.4 * 100) + (0.3 * 100) + (0.3 * 10) = 40 + 30 + 3 = 73

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(73, score);
        }

        [Fact]
        public void CalculateCreditScore_WorstScenario_ReturnsMinScore()
        {
            // Arrange: No payment history, maxed out utilization, no credit history
            double paymentHistory = 0;
            double creditUtilization = 100;
            double ageOfCreditHistory = 0;
            // Expected: (0.4 * 0) + (0.3 * 0) + (0.3 * 0) = 0

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(0, score);
        }

        [Fact]
        public void CalculateCreditScore_AgeCapAt10Years_CalculatesCorrectly()
        {
            // Arrange: Test that age is capped at 10 years for scoring
            double paymentHistory = 80;
            double creditUtilization = 50;
            double ageOfCreditHistory = 30; // Should be capped at 10
            // Expected: (0.4 * 80) + (0.3 * 50) + (0.3 * 10) = 32 + 15 + 3 = 50

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(50, score);
        }

        [Fact]
        public void CalculateCreditScore_RoundingDown_ReturnsCorrectInteger()
        {
            // Arrange: Score that should round down
            double paymentHistory = 50;
            double creditUtilization = 50;
            double ageOfCreditHistory = 4;
            // Expected: (0.4 * 50) + (0.3 * 50) + (0.3 * 4) = 20 + 15 + 1.2 = 36.2 ≈ 36

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(36, score);
        }

        [Fact]
        public void CalculateCreditScore_RoundingUp_ReturnsCorrectInteger()
        {
            // Arrange: Score that should round up
            double paymentHistory = 50;
            double creditUtilization = 49;
            double ageOfCreditHistory = 5;
            // Expected: (0.4 * 50) + (0.3 * 51) + (0.3 * 5) = 20 + 15.3 + 1.5 = 36.8 ≈ 37

            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, creditUtilization, ageOfCreditHistory);

            // Assert
            Assert.Equal(37, score);
        }

        #endregion

        #region Input Validation Tests

        [Fact]
        public void CalculateCreditScore_NegativePaymentHistory_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _assessor.CalculateCreditScore(-10, 50, 5));
        }

        [Fact]
        public void CalculateCreditScore_PaymentHistoryOver100_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _assessor.CalculateCreditScore(101, 50, 5));
        }

        [Fact]
        public void CalculateCreditScore_NegativeCreditUtilization_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _assessor.CalculateCreditScore(80, -5, 5));
        }

        [Fact]
        public void CalculateCreditScore_CreditUtilizationOver100_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _assessor.CalculateCreditScore(80, 101, 5));
        }

        [Fact]
        public void CalculateCreditScore_NegativeAgeOfCreditHistory_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _assessor.CalculateCreditScore(80, 50, -1));
        }

        #endregion

        #region Edge Case Tests

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(100)]
        public void CalculateCreditScore_BoundaryPaymentHistory_ReturnsValidScore(double paymentHistory)
        {
            // Act
            int score = _assessor.CalculateCreditScore(paymentHistory, 50, 5);

            // Assert
            Assert.True(score >= 0 && score <= 100, "Score should be within valid range");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(100)]
        public void CalculateCreditScore_BoundaryCreditUtilization_ReturnsValidScore(double utilization)
        {
            // Act
            int score = _assessor.CalculateCreditScore(80, utilization, 5);

            // Assert
            Assert.True(score >= 0 && score <= 100, "Score should be within valid range");
        }

        #endregion

        #region Customer Evaluation Tests

        [Fact]
        public void EvaluateCustomer_ValidCustomer_ReturnsCorrectResult()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerId = 1,
                Name = "Test User",
                PaymentHistory = 85,
                CreditUtilization = 40,
                AgeOfCreditHistory = 5
            };

            // Act
            var result = _assessor.EvaluateCustomer(customer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CustomerId);
            Assert.Equal("Test User", result.Name);
            Assert.True(result.CreditScore > 0);
            Assert.NotEmpty(result.RiskStatus);
        }

        [Fact]
        public void EvaluateCustomer_HighRiskCustomer_MarksAsHighRisk()
        {
            // Arrange: Customer with score below 50
            var customer = new Customer
            {
                CustomerId = 2,
                Name = "Risky Customer",
                PaymentHistory = 40,
                CreditUtilization = 95,
                AgeOfCreditHistory = 1
            };

            // Act
            var result = _assessor.EvaluateCustomer(customer);

            // Assert
            Assert.Equal("High Risk", result.RiskStatus);
        }

        [Fact]
        public void EvaluateCustomer_LowRiskCustomer_MarksAsLowRisk()
        {
            // Arrange: Customer with score 50 or above
            var customer = new Customer
            {
                CustomerId = 3,
                Name = "Good Customer",
                PaymentHistory = 90,
                CreditUtilization = 30,
                AgeOfCreditHistory = 8
            };

            // Act
            var result = _assessor.EvaluateCustomer(customer);

            // Assert
            Assert.Equal("Low Risk", result.RiskStatus);
        }

        [Fact]
        public void GetHighRiskCustomers_WithMixedRisks_ReturnsOnlyHighRisk()
        {
            // Arrange
            var results = new List<CreditEvaluationResult>
            {
                new CreditEvaluationResult { CustomerId = 1, Name = "Alice", CreditScore = 70, RiskStatus = "Low Risk" },
                new CreditEvaluationResult { CustomerId = 2, Name = "Bob", CreditScore = 35, RiskStatus = "High Risk" },
                new CreditEvaluationResult { CustomerId = 3, Name = "Charlie", CreditScore = 40, RiskStatus = "High Risk" },
                new CreditEvaluationResult { CustomerId = 4, Name = "Diana", CreditScore = 65, RiskStatus = "Low Risk" }
            };

            // Act
            var highRiskOnly = _assessor.GetHighRiskCustomers(results);

            // Assert
            Assert.Equal(2, highRiskOnly.Count);
            Assert.All(highRiskOnly, r => Assert.Equal("High Risk", r.RiskStatus));
        }

        #endregion
    }
}
