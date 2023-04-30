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
            try
            {
                await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
                int userId = await _customAuthenticationStateProvider.GetUserId();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                return await _httpClient.GetFromJsonAsync<EmployeeDetails>($"{BaseApiUrl}/{userId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<List<EmployeeHistory>> GetEmployeeHistoryAsync()
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				int userId = await _customAuthenticationStateProvider.GetUserId();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                return await _httpClient.GetFromJsonAsync<List<EmployeeHistory>>($"{BaseApiUrl}/employeeHistory/{userId}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task AddVacationAsync(Vacation vacation)
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                vacation.UserId = await _customAuthenticationStateProvider.GetUserId();
                var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
                httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
                await _httpClient.SendAsync(httpPostRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public async Task DeleteVacationRequestAndRestoreVacationAsync(int vacationId)
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                var httpDeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseApiUrl}/{vacationId}");
                await _httpClient.SendAsync(httpDeleteRequest);
            }
            catch (Exception e)
            {
				Console.WriteLine(e);
			}
        }
        public async Task UpdateVacationStatusAsync(Vacation vacation)
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/updateVacationStatus");
                httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
                await _httpClient.SendAsync(httpPutRequest);
            }
            catch (Exception e)
            {
				Console.WriteLine(e);
			}
        }
        public async Task<int> CalculateTotalVacationForGivenPeriodAsync (List<DateTime> vacation)
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                var httpGetRequest = new HttpRequestMessage(HttpMethod.Get, BaseApiUrl);
                httpGetRequest.Content = new StringContent(JsonSerializer.Serialize(vacation), Encoding.UTF8, "application/json");
                var response = await _httpClient.SendAsync(httpGetRequest);
                return int.Parse(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception e)
            {
				Console.WriteLine(e);
                return -1;
			}
        }
		public async Task CloseSickLeaveStatusAsync(SickLeave sickLeave)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/closeSickLeave");
				httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(sickLeave), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPutRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
