using DomainModel.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Service
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly string BaseApiUrl = "https://localhost:7058/api/User";

        public UserService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userId), Encoding.UTF8, "application/json");
            //httpPostRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpResponseMessage = await _httpClient.SendAsync(httpPostRequest);
            var userAsString = await httpResponseMessage.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(userAsString);
            return user;
        }
    }
}
