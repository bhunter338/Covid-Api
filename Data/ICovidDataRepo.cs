using System.Collections.Generic;
using Covid_Api.Models;

namespace Covid_Api.Data
{
    public interface ICovidDataRepo
    {
        DailyData GetTotalDataByCountry(string country);
        List<string> GetCountries();

        List<DailyData> GetHistroicalDataByCountry(string country);


    }
}