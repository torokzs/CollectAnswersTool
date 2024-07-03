// Purpose of this tool to read questions from a CSV file and collect answers from LLM
// and write answers back to the CSV file. This file can be used then to evaluate the answers.

using CsvHelper.Configuration.Attributes;

namespace CollectAnswersTool
{
    public class CsvRecord
    {
        [Index(0)]
        public required string Question { get; set; }
        [Index(1)]
        public required string GroundTruth { get; set; }
        [Index(2)]
        public string? ModelAnswer { get; set; }
    }
}