using System.Collections.Generic;
using Covid_Api.Models;

namespace Covid_Api.Data
{
    public interface ICovidDataRepo
    {
        TotalData GetTotalDataByCountry(string country);
        List<string> GetCountries();


    }
}