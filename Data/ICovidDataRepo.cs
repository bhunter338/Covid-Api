using Covid_Api.Models;

namespace Covid_Api.Data
{
    public interface ICovidDataRepo
    {
        TotalData GetTotalData(string country);
        TotalData GetDailyTotalData(string country);


    }
}