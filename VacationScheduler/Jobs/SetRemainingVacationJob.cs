using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationScheduler.Jobs
{
    public class SetRemainingVacationJob : IJob
    {
        HttpClient _httpClient = new HttpClient();
        private readonly string BaseApiUrl = "https://localhost:7058/api/Employee";
        public async Task Execute(IJobExecutionContext context)
        {
            var currentDate = DateTime.Today.Date;
            if(currentDate.Day.Equals(1) && currentDate.Month.Equals(1))
            {
                await Console.Out.WriteLineAsync("Updating Employee Remaining Vacation.");
                await SetRemainingVacation();
                await Console.Out.WriteLineAsync("Employee Remaining Vacation Updated!");
            }
        }
        public async Task SetRemainingVacation()
        {
            var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, BaseApiUrl + "/updateVacationStatusViaScheduler");
            await _httpClient.SendAsync(httpPutRequest);
        }
    }
}
