using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationScheduler.Jobs
{
    public class SetVacationStatusJob : IJob
    {
        HttpClient _httpClient = new HttpClient();
        private readonly string BaseApiUrl = "https://localhost:7058/api/Employee";

        public async Task Execute(IJobExecutionContext context)
        {
            await Console.Out.WriteLineAsync("Updating Employee Vacation Status...");
            var currentDate = DateTime.Today.Date;
            await SetVacationStatus(currentDate);
            await Console.Out.WriteLineAsync("Employee Vacation Status Updated!");
        }

        public async Task SetVacationStatus(DateTime currentDate)
        {
            var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, BaseApiUrl + "/updateVacationStatus");
            httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(currentDate), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPutRequest);
        }
    }
}
