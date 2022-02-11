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
        private readonly CovidAppContext _context;

        public CovidDataRepo(CovidAppContext context)
        {
            _context = context;
        }
        public List<string> GetCountries()
        {
            var data = GetSiteData();
            List<string> countries = data.AsEnumerable().Select(x => x["Country,Other"].ToString()).Where(i => i != "World").OrderBy(x => x).ToList();

            countries.Insert(0, "World");

            // foreach (var country in countries)
            // {
            //     _context.countries.Add(new Country { Name = country });
            // }

            // _context.SaveChanges();


            return countries;
        }

        public DailyData GetTotalDataByCountry(string country)
        {
            var data = GetSiteData();
            var selectedData = data.Select("[Country,Other] = '" + country + "'");

            if (selectedData.Count() == 0)
            {
                return null;
            }

            int totalCases = 0;
            int totalRecovered = 0;
            int totaldeaths = 0;
            int activeCases = 0;
            int serious = 0;
            int casesPer = 0;

            string name = selectedData.FirstOrDefault().Field<string>("Country,Other").Trim();
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("TotalCases").Trim().Replace(",", string.Empty), out totalCases);
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("TotalRecovered").Trim().Replace(",", string.Empty), out totalRecovered);
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("TotalDeaths").Trim().Replace(",", string.Empty), out totaldeaths);
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("ActiveCases").Trim().Replace(",", string.Empty), out activeCases);
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("Serious,Critical").Trim().Replace(",", string.Empty), out serious);
            Int32.TryParse(selectedData.FirstOrDefault().Field<string>("Tot&nbsp;Cases/1M pop").Trim().Replace(",", string.Empty), out casesPer);

            return new DailyData
            {
                CountryName = name,
                TotalConfirmed = totalCases,
                TotalRecovered = totalRecovered,
                TotalDeaths = totaldeaths,
                ActiveCases = activeCases,
                CasesPer1MPopulation = casesPer,
                Serious = serious
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



            var cols = new List<string>() { "TotalCases", "TotalDeaths", "TotalRecovered", "Country,Other", "ActiveCases", "Serious,Critical", "Tot&nbsp;Cases/1M pop" };

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