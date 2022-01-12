using System.Collections.Generic;
using System.Data;
using System.Linq;
using Covid_Api.Models;
using HtmlAgilityPack;

namespace Covid_Api.Data
{
    public class CovidDataRepo : ICovidDataRepo
    {
        public TotalData GetDailyTotalData(string country)
        {


            return new TotalData { Name = country, TotalConfirmed = 10, TotalRecovered = 20, TotalDeaths = 40 };
        }

        public TotalData GetTotalData(string country)
        {

            string url = "https://www.worldometers.info/coronavirus/";
            var web = new HtmlWeb();
            var doc = web.Load(url).DocumentNode;

            var th = doc.SelectNodes("//*[@id=\"main_table_countries_today\"]//tr//th");

            // var headers = th.Select(i => i.InnerText).Where(x => new List<string>() { "TotalCases", "TotalDeaths", "TotalRecovered", "Country,Other" }.Contains(x));

            DataTable table = new DataTable();
            foreach (var header in th)
                table.Columns.Add(header.InnerText);

            var rows = doc.SelectNodes("//*[@id=\"main_table_countries_today\"]//tr[not(contains(@class,'row_continent')) and td]");


            foreach (var row in rows)
                table.Rows.Add(row.SelectNodes("td").Select(td => td.InnerText).ToArray());



            return new TotalData { TotalConfirmed = 1000, TotalRecovered = 20, TotalDeaths = 40 };
        }
    }
}