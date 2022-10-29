using System.Text.Json;
using System.Text;
using DomainModel.Models;

namespace VacationDaysCalculatorBlazorServer.Services
{
    public class LogInService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseApiUrl = "https://localhost:7058/api/Login";
        public LogInService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SendUserAsync(UserLogin userLogin)
        {
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPostRequest);
        }

    }
}
