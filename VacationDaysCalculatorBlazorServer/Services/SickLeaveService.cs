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
	}
}
