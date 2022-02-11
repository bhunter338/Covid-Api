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
            return _context.countries.Select(i => i.Name).ToList();
        }

        public DailyData GetTotalDataByCountry(string country)
        {
            int now = 0;
            Int32.TryParse(DateTime.Now.ToString("yyyyMMdd"), out now);

            return _context.dailyDatas.FirstOrDefault(p => p.CountryName == country && p.date == now);
        }
    }
}