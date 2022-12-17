using DomainModel.DtoModels;
using System.Net.Http.Headers;

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
        public async Task<AdminDetails> GetAdminDetailsAsync(int userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<AdminDetails>($"{BaseApiUrl}/{userId}");
        }
    }
}
