namespace Covid_Api.Dtos
{
    public class DailyDataReadDto
    {
        public int Id { get; set; }
        public string CountryName { get; set; }

        public int date { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalRecovered { get; set; }

        public int TotalDeaths { get; set; }
        public int ActiveCases { get; set; }
        public int Serious { get; set; }

        public int CasesPer1MPopulation { get; set; }

    }
}