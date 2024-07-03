namespace CAT.Common
{
    public class ResponseModel
    {
        public string? Role { get; set; }
        public string? ContentPath { get; set; }
        public required string Result { get; set; }
        public DateTime MessageDate { get; set; }
        public int DailyRequestCount { get; set; }
        public int DailyRemainingRequestCount { get; set; }
    }
}
