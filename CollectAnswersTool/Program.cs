using CsvHelper;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;

namespace CollectAnswersTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Initialize main variables
            List<CsvRecord> records = [];

            // Read CSV file path from config file
            string? csvFilePath = ConfigurationManager.AppSettings["CSVFilePath"];
            Console.WriteLine("Reading CSV File: " + csvFilePath);
            if (string.IsNullOrEmpty(csvFilePath))
            {
                Console.WriteLine("ERROR: CSVFilePath is not configured properly, please check configuration settings.");
                return;
            }

            // Read questions from CSV file and collect answers from LLM
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<CsvRecord>().ToList();
                foreach (var record in records)
                {
                    Console.WriteLine("Question: " + record.Question);

                    // Removing new lines to prevent a CSV schema recognition problem when uploading for evaluation
                    record.Question = record.Question.Replace("\r", " ").Replace("\n", " ");
                    record.GroundTruth = record.GroundTruth.Replace("\r", " ").Replace("\n", " ");

                    // Collect answer from LLM
                    var watch = Stopwatch.StartNew();
                    watch.Start(); string answer = ChatbotClient.PostQuestionAsync(record.Question).Result;
                    record.ModelAnswer = answer.Replace("\r", " ").Replace("\n", " ");
                    watch.Stop();

                    Console.WriteLine("Answer: " + record.ModelAnswer);
                    Console.WriteLine("Elapsed time: " + watch.ElapsedMilliseconds + " ms");
                    Console.WriteLine("---------------------------------------------");
                }
            }

            // Save CSV file with the given answers
            using (var writer = new StreamWriter(csvFilePath))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(records);
            }
            Console.WriteLine("Answers written back to CSV file.");
        }
    }
}