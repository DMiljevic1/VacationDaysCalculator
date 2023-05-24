using DomainModel.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VacationDaysCalculatorBlazorServer.Services
{
	public class SickLeaveService
	{
		private readonly HttpClient _httpClient;
		private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
		private readonly string BaseApiUrl = "https://localhost:7058/api/SickLeave";

		public SickLeaveService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider)
		{
			_httpClient = httpClient;
			_customAuthenticationStateProvider = customAuthenticationStateProvider;
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
