using DomainModel.DtoModels;
using DomainModel.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace VacationDaysCalculatorBlazorServer.Services
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
        public async Task<string> GetUserPassword()
        {
            try
            {
                await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                int userId = await _customAuthenticationStateProvider.GetUserId();
                var httpPostRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseApiUrl}/getPassword/{userId}");
                httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userId), Encoding.UTF8, "application/json");
                var httpResponseMessage = await _httpClient.SendAsync(httpPostRequest);
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "";
            }
        }
        public async Task ChangePasswordAsync(Password password)
        {
            try
            {
				await _customAuthenticationStateProvider.DeleteExpiredTokenFromLocalStorage();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _customAuthenticationStateProvider.GetTokenAsync());
                password.UserId = await _customAuthenticationStateProvider.GetUserId();
                var httpPutRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseApiUrl}/changePassword");
                httpPutRequest.Content = new StringContent(JsonSerializer.Serialize(password), Encoding.UTF8, "application/json");
                await _httpClient.SendAsync(httpPutRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
