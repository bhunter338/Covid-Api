using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Covid_Api.Data;
using Covid_Api.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace Covid_Api.ScheduledTask
{
    public class InsertService : HostedService
    {
        HttpClient restClient;
        private readonly IServiceProvider _serviceProvider;

        public InsertService(IServiceProvider serviceProvider)
        {
            restClient = new HttpClient();
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                DataInsertUpdate();
                DeleteOldData();

                await Task.Delay(TimeSpan.FromMinutes(30), cToken);
            }
        }

        private void DeleteOldData()
        {
            int date;
            Int32.TryParse(DateTime.Now.AddDays(-31).ToString("yyyyMMdd"), out date);
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<CovidAppContext>();
                List<DailyData> entitiesToRemove = context.dailyDatas.Where(i => i.date < date).ToList();
                context.dailyDatas.RemoveRange(entitiesToRemove);
                context.SaveChanges();
            }

        }

        private void DataInsertUpdate()
        {
            int now = 0;
            Int32.TryParse(DateTime.Now.ToString("yyyyMMdd"), out now);


            var data = GetSiteData();

            using (var scope = _serviceProvider.CreateScope()) // this will use `IServiceScopeFactory` internally
            {
                var context = scope.ServiceProvider.GetService<CovidAppContext>();

                foreach (DataRow row in data.Rows)
                {
                    var entity = context.dailyDatas.Where(p => p.CountryName == row.Field<string>("Country,Other").ToString() && p.date == now);
                    int count = entity.Count();

                    int totalCases, totalRecovered, totaldeaths, activeCases, serious, casesPer = 0;

                    string name = row.Field<string>("Country,Other").Trim();
                    Int32.TryParse(row.Field<string>("TotalCases").Trim().Replace(",", string.Empty), out totalCases);
                    Int32.TryParse(row.Field<string>("TotalRecovered").Trim().Replace(",", string.Empty), out totalRecovered);
                    Int32.TryParse(row.Field<string>("TotalDeaths").Trim().Replace(",", string.Empty), out totaldeaths);
                    Int32.TryParse(row.Field<string>("ActiveCases").Trim().Replace(",", string.Empty), out activeCases);
                    Int32.TryParse(row.Field<string>("Serious,Critical").Trim().Replace(",", string.Empty), out serious);
                    Int32.TryParse(row.Field<string>("Tot&nbsp;Cases/1M pop").Trim().Replace(",", string.Empty), out casesPer);

                    if (count == 0) //no data inserted today. insert
                    {
                        DailyData dailyData = new DailyData
                        {
                            CountryName = name,
                            TotalConfirmed = totalCases,
                            TotalRecovered = totalRecovered,
                            TotalDeaths = totaldeaths,
                            ActiveCases = activeCases,
                            Serious = serious,
                            CasesPer1MPopulation = casesPer,
                            date = now,
                            CreateDate = DateTime.Now.ToString("yyyyMMddHHmm")
                        };

                        context.dailyDatas.Add(dailyData);

                    }
                    else // update 
                    {
                        var updateEntity = entity.FirstOrDefault();
                        if (updateEntity != null)
                        {
                            updateEntity.TotalConfirmed = totalCases;
                            updateEntity.TotalRecovered = totalRecovered;
                            updateEntity.TotalDeaths = totaldeaths;
                            updateEntity.ActiveCases = activeCases;
                            updateEntity.Serious = serious;
                            updateEntity.CasesPer1MPopulation = casesPer;
                            updateEntity.UpdateDate = DateTime.Now.ToString("yyyyMMddHHmm");
                        }

                    }
                }
                context.SaveChanges();
            }
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