using Covid_Api.Models;

namespace Covid_Api.Data
{
    public class CovidDataRepo : ICovidDataRepo
    {
        public TotalData GetDailyTotalData(string country)
        {
            return new TotalData { TotalConfirmed = 10, TotalRecovered = 20, TotalDeaths = 40 };
        }

        public TotalData GetTotalData(string country)
        {
            return new TotalData { TotalConfirmed = 1000, TotalRecovered = 20, TotalDeaths = 40 };
        }
    }
}