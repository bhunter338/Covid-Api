using System.Data;
using Covid_Api.Models;
using HtmlAgilityPack;

namespace Covid_Api.Data
{
    public class CovidDataRepo : ICovidDataRepo
    {
        public TotalData GetDailyTotalData(string country)
        {
            string url = "https://www.worldometers.info/coronavirus/";
            var web = new HtmlWeb();
            var doc = web.Load(url).DocumentNode;

            var headers = doc.SelectNodes("//tr/th");

            DataTable table = new DataTable();
            foreach (HtmlNode header in headers)
                table.Columns.Add(header.InnerText);

            return new TotalData { Name = country, TotalConfirmed = 10, TotalRecovered = 20, TotalDeaths = 40 };
        }

        public TotalData GetTotalData(string country)
        {
            return new TotalData { TotalConfirmed = 1000, TotalRecovered = 20, TotalDeaths = 40 };
        }
    }
}