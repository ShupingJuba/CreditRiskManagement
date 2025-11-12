namespace CreditRiskManagement.Services
{
    using System.Text.Json;
    using CreditRiskManagement.Models;

    /// <summary>
    /// Service for generating and saving credit reports.
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// Generates a formatted console report of credit evaluations.
        /// </summary>
        /// <param name="results">List of evaluation results to report.</param>
        public void GenerateConsoleReport(List<CreditEvaluationResult> results)
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("CREDIT RISK ASSESSMENT REPORT");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine();

            if (!results.Any())
            {
                Console.WriteLine("No customers to report.");
                return;
            }

            // Print header
            Console.WriteLine($"{"ID",-4} | {"Name",-15} | {"Credit Score",-3} | {"Risk Status",-10}");
            Console.WriteLine(new string('-', 80));

            // Print each customer
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.WriteLine(new string('-', 80));

            // Print summary statistics
            int highRiskCount = results.Count(r => r.RiskStatus == "High Risk");
            int lowRiskCount = results.Count(r => r.RiskStatus == "Low Risk");
            double avgScore = results.Average(r => r.CreditScore);

            Console.WriteLine();
            Console.WriteLine($"Total Customers: {results.Count}");
            Console.WriteLine($"High Risk: {highRiskCount}");
            Console.WriteLine($"Low Risk: {lowRiskCount}");
            Console.WriteLine($"Average Credit Score: {avgScore:F2}");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine();
        }

        /// <summary>
        /// Saves credit evaluation results to a JSON file.
        /// </summary>
        /// <param name="results">List of evaluation results to save.</param>
        /// <param name="filePath">Path where the JSON file will be saved.</param>
        public void SaveReportAsJson(List<CreditEvaluationResult> results, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var reportData = new
                {
                    GeneratedAt = DateTime.UtcNow,
                    TotalCustomers = results.Count,
                    HighRiskCount = results.Count(r => r.RiskStatus == "High Risk"),
                    LowRiskCount = results.Count(r => r.RiskStatus == "Low Risk"),
                    AverageScore = results.Any() ? results.Average(r => r.CreditScore) : 0,
                    Customers = results
                };

                string json = JsonSerializer.Serialize(reportData, options);
                File.WriteAllText(filePath, json);

                Console.WriteLine($"Report saved successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving report: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Loads customer data from a JSON file.
        /// </summary>
        /// <param name="filePath">Path to the JSON file containing customer data.</param>
        /// <returns>List of customers loaded from the file.</returns>
        public List<Customer> LoadCustomersFromJson(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Customer data file not found: {filePath}");

                string json = File.ReadAllText(filePath);
                var customers = JsonSerializer.Deserialize<List<Customer>>(json);

                return customers ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading customer data: {ex.Message}");
                throw;
            }
        }
    }
}
