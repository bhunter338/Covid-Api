namespace Covid_Api.Models
{
    public class TotalData
    {
        public int TotalConfirmed { get; set; }
        public int TotalRecovered { get; set; }

        public int TotalDeaths { get; set; }
        public int DailyConfirmed { get; set; }
        public int DailyRecovered { get; set; }

        public int DailyDeaths { get; set; }

    }
}