namespace Covid_Api.Models
{
    public class TotalData
    {
        public string Name { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalRecovered { get; set; }

        public int TotalDeaths { get; set; }
        public int ActiveCases { get; set; }
        public int Serious { get; set; }

        public int CasesPer1MPopulation { get; set; }

    }
}