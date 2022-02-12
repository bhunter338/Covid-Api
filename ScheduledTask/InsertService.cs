using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Covid_Api.ScheduledTask
{
    public class InsertService : HostedService
    {
        HttpClient restClient;
        string icndbUrl = "http://api.icndb.com/jokes/random";
        public InsertService()
        {
            restClient = new HttpClient();
        }
        protected override async Task ExecuteAsync(CancellationToken cToken)
        {
            while (!cToken.IsCancellationRequested)
            {
                var response = await restClient.GetAsync(icndbUrl, cToken);
                if (response.IsSuccessStatusCode)
                {
                    var fact = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"{DateTime.Now.ToString()}\n{fact}");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), cToken);
            }
        }
    }
}