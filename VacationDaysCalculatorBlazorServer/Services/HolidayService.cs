using DomainModel.DtoModels;
using DomainModel.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VacationDaysCalculatorBlazorServer.Services
{
	public class HolidayService
	{
		private readonly HttpClient _httpClient;
		private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
		private readonly string BaseApiUrl = "https://localhost:7058/api/Holiday";
		public HolidayService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
		{
			_httpClient = httpClient;
			_customAuthenticationStateProvider = customAuthenticationStateProvider;
		}
		public async Task<List<Holiday>> GetHolidaysAsync()
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				return await _httpClient.GetFromJsonAsync<List<Holiday>>(BaseApiUrl);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				return null;
			}
		}
		public async Task DeleteHolidayAsync(int holidayId)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpDeleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseApiUrl}/{holidayId}");
				await _httpClient.SendAsync(httpDeleteRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public async Task UpdateHolidayAsync(Holiday holiday)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/updateHoliday");
				httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(holiday), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPutRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
		public async Task AddHolidayAsync(Holiday holiday)
		{
			try
			{
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
				var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseApiUrl}/addHoliday");
				httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(holiday), Encoding.UTF8, "application/json");
				await _httpClient.SendAsync(httpPostRequest);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}
