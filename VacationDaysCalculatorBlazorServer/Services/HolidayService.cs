using DomainModel.DtoModels;
using DomainModel.Models;
using System.Net.Http.Headers;

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
	}
}
