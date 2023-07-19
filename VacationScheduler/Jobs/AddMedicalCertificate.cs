using DomainModel.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VacationScheduler.Jobs
{
    public class AddMedicalCertificate : IJob
    {
        HttpClient _httpClient = new HttpClient();
        private readonly string BaseApiUrl = "https://localhost:7058/api/SickLeave";
        public async Task Execute(IJobExecutionContext context)
        {
            var currentDate = DateTime.Today.Date;
            if(currentDate.Day.Equals(1))
            {
                await Console.Out.WriteLineAsync("Adding Medical Certificate for current month...");
                await AddMedicalCertificates(currentDate);
                await Console.Out.WriteLineAsync("Adding Medical Certificate by scheduler completed succesfully!");
            }
        }
        private async Task AddMedicalCertificates(DateTime currentDate)
        {
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl + "/addMedCertViaScheduler");
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(currentDate), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPostRequest);
        }
    }
}
