using CreditRiskManagement.Models;
using CreditRiskManagement.Services;

namespace CreditRiskManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize services
                var assessor = new CreditRiskAssessor();
                var reportGenerator = new ReportGenerator();

                // Load customer data from JSON file
                string customerDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "customers.json");

                Console.WriteLine("Loading customer data...");
                var customers = reportGenerator.LoadCustomersFromJson(customerDataPath);

                if (!customers.Any())
                {
                    Console.WriteLine("No customers found in data file.");
                    return;
                }

                Console.WriteLine($"Loaded {customers.Count} customers.");

                // Evaluate all customers
                Console.WriteLine("\nEvaluating customer credit scores...");
                var evaluationResults = assessor.EvaluateCustomers(customers);

                // Generate console report
                reportGenerator.GenerateConsoleReport(evaluationResults);

                // Identify and display high-risk customers
                var highRiskCustomers = assessor.GetHighRiskCustomers(evaluationResults);
                if (highRiskCustomers.Any())
                {
                    Console.WriteLine("\n⚠️  HIGH-RISK CUSTOMERS ALERT:");
                    Console.WriteLine(new string('-', 80));
                    foreach (var customer in highRiskCustomers)
                    {
                        Console.WriteLine($"  • {customer.Name} (ID: {customer.CustomerId}) - Score: {customer.CreditScore}");
                    }
                    Console.WriteLine(new string('-', 80));
                }

                // Save report to JSON file
                string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", $"credit_report_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                Directory.CreateDirectory(Path.GetDirectoryName(reportPath)!);
                reportGenerator.SaveReportAsJson(evaluationResults, reportPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Application Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
