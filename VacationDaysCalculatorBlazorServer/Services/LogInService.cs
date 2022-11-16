using System.Text.Json;
using System.Text;
using DomainModel.Models;

namespace VacationDaysCalculatorBlazorServer.Services
{
    public class LogInService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseApiUrl = "https://localhost:7058/api/Login";
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        public LogInService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _customAuthenticationStateProvider=customAuthenticationStateProvider;
        }

        public async Task SendUserAsync(UserLogin userLogin)
        {
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
            var httpResponseMessage = await _httpClient.SendAsync(httpPostRequest);

            var jwtToken = httpResponseMessage.Content.ReadAsStringAsync();
            if(jwtToken.Result != "User not found")
                await _customAuthenticationStateProvider.SetTokenAsync(jwtToken.Result);
        }

    }
}
