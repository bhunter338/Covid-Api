using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid_Api.Models;

namespace Covid_Api.Data
{
    public class SqlCovidRepo : ICovidDataRepo
    {
        private readonly CovidAppContext _context;

        public SqlCovidRepo(CovidAppContext context)
        {
            _context = context;
        }
        public List<string> GetCountries()
        {
            var result = _context.countries.Select(i => i.Name).OrderBy(x => x).ToList();
            result.Insert(0, "World");
            return result;
        }

        public DailyData GetTotalDataByCountry(string country)
        {
            int now = 0;
            Int32.TryParse(DateTime.Now.ToString("yyyyMMdd"), out now);

            return _context.dailyDatas.FirstOrDefault(p => p.CountryName == country && p.date == now);
        }
    }
}