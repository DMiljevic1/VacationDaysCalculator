using DomainModel.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using VacationDaysCalculatorBlazorServer.Services;
using DomainModel.DtoModels;

namespace VacationDaysCalculatorBlazorServer.Service
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly string BaseApiUrl = "https://localhost:7058/api/Employee";

        public EmployeeService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
        {
            _httpClient = httpClient;
            _customAuthenticationStateProvider = customAuthenticationStateProvider;
        }
        public async Task<EmployeeDetails> GetEmployeeDetailsAsync(int userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<EmployeeDetails>($"{BaseApiUrl}/{userId}");
        }

        public async Task<List<EmployeeHistory>> GetEmployeeHistoryAsync(int userId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<List<EmployeeHistory>>($"{BaseApiUrl}/employeeHistory/{userId}");
        }
        public async Task AddVacationAsync(VacationDays vacation)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPostRequest);
        }
    }
}
