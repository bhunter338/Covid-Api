using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Covid_Api.Models;
using HtmlAgilityPack;

namespace Covid_Api.Data
{
    public class CovidDataRepo : ICovidDataRepo
    {
        public List<string> GetCountries()
        {
            var data = GetSiteData();
            List<string> countries = data.AsEnumerable().Select(x => x["Country,Other"].ToString()).Where(i => i != "World").OrderBy(x => x).ToList();

            countries.Insert(0, "World");

            return countries;
        }

        public TotalData GetTotalDataByCountry(string country)
        {
            var data = GetSiteData();
            var selectedData = data.Select("[Country,Other] = '" + country + "'");

            if (selectedData.Count() == 0)
            {
                return null;
            }

            string name = selectedData.FirstOrDefault().Field<string>("Country,Other").Trim();
            int totalCases = Int32.Parse(selectedData.FirstOrDefault().Field<string>("TotalCases").Trim().Replace(",", string.Empty));
            int totalRecovered = Int32.Parse(selectedData.FirstOrDefault().Field<string>("TotalRecovered").Trim().Replace(",", string.Empty));
            int totaldeaths = Int32.Parse(selectedData.FirstOrDefault().Field<string>("TotalDeaths").Trim().Replace(",", string.Empty));
            int activeCases = Int32.Parse(selectedData.FirstOrDefault().Field<string>("ActiveCases").Trim().Replace(",", string.Empty));
            int totalTests = Int32.Parse(selectedData.FirstOrDefault().Field<string>("TotalTests").Trim().Replace(",", string.Empty));
            int population = Int32.Parse(selectedData.FirstOrDefault().Field<string>("Population").Trim().Replace(",", string.Empty));

            return new TotalData
            {
                Name = name,
                TotalConfirmed = totalCases,
                TotalRecovered = totalRecovered,
                TotalDeaths = totaldeaths,
                ActiveCases = activeCases,
                TotalTests = totalTests,
                Population = population
            };
        }

        private DataTable GetSiteData()
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



            var cols = new List<string>() { "TotalCases", "TotalDeaths", "TotalRecovered", "Country,Other", "ActiveCases", "TotalTests", "Population" };

            var dtClone = table.Clone();

            foreach (var col in dtClone.Columns)
            {
                if (!cols.Contains(col.ToString()))
                {
                    table.Columns.Remove(col.ToString());
                }

            }

            return table;
        }
    }
}