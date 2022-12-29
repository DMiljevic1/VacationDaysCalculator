using DomainModel.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using VacationDaysCalculatorBlazorServer.Services;
using DomainModel.DtoModels;
using System;

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
        public async Task<EmployeeDetails> GetEmployeeDetailsAsync()
        {
            int userId = await _customAuthenticationStateProvider.GetUserId();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<EmployeeDetails>($"{BaseApiUrl}/{userId}");
        }

        public async Task<List<EmployeeHistory>> GetEmployeeHistoryAsync()
        {
            int userId = await _customAuthenticationStateProvider.GetUserId();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            return await _httpClient.GetFromJsonAsync<List<EmployeeHistory>>($"{BaseApiUrl}/employeeHistory/{userId}");
        }
        public async Task AddVacationAsync(Vacation vacation)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            vacation.UserId = await _customAuthenticationStateProvider.GetUserId();
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
            httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPostRequest);
        }
        public async Task DeleteVacationRequestAndRestoreVacationAsync(int vacationId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpDeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseApiUrl}/{vacationId}");
            await _httpClient.SendAsync(httpDeleteRequest);
        }
        public async Task UpdateEmployeeVacationStatusAsync(Vacation vacation)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, BaseApiUrl);
            httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
            await _httpClient.SendAsync(httpPutRequest);
        }
        public async Task<int> CalculateTotalVacationForGivenPeriodAsync (List<DateTime> vacation)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
            var httpGetRequest = new HttpRequestMessage(HttpMethod.Get, BaseApiUrl);
            httpGetRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(httpGetRequest);
            return int.Parse(response.Content.ReadAsStringAsync().Result);
        }
    }
}
