using DomainModel.DtoModels;
using DomainModel.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VacationDaysCalculatorBlazorServer.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly string BaseApiUrl = "https://localhost:7058/api/Admin";
        public AdminService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }
        public async Task<AdminDetails> GetAdminDetailsAsync()
        {
            int userId = await _customAuthenticationStateProvider.GetUserId();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<AdminDetails>($"{BaseApiUrl}/{userId}");
        }
        public async Task AddUserAsync(UserDetails userDetails)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userDetails), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPostRequest);
        }
        public async Task<List<Vacation>> GetApprovedVacations()
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<List<Vacation>>($"{BaseApiUrl}/approvedVacations");
        }
    }
}
