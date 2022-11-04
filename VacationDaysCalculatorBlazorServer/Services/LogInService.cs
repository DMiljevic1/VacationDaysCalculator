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

        public async Task<String> SendUserAsync(UserLogin userLogin)
        {
            var httpGetRequest = new HttpRequestMessage(HttpMethod.Get, BaseApiUrl);
            httpGetRequest.Content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(httpGetRequest);
            if(response.IsSuccessStatusCode)
            {
                String token = await response.Content.ReadAsStringAsync();
                return token;
            }
            return null;
        }
    }
}
