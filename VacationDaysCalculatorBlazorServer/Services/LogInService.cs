using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Pages.RazorPageBases;
using DomainModel.DtoModels;
using System.Security.Claims;

namespace VacationDaysCalculatorBlazorServer.Services
{
    public class LogInService
    {
        private readonly HttpClient _httpClient;
        private readonly string BaseApiUrl = "https://localhost:7058/api/Login";
        private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
        private readonly NavigationManager _navigationManager;
        public LogInService(HttpClient httpClient, CustomAuthenticationStateProvider customAuthenticationStateProvider, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _customAuthenticationStateProvider=customAuthenticationStateProvider;
            _navigationManager = navigationManager;
        }

        public async Task SendUserAsync(UserLogin userLogin)
        {
            if(userLogin.UserName == null || userLogin.Password == null)
                LoginPageBase.message = "Username or password cannot be empty!";
            else
            {
                var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, BaseApiUrl);
                httpPostRequest.Content = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");
                var httpResponseMessage = await _httpClient.SendAsync(httpPostRequest);

                var jwtToken = httpResponseMessage.Content.ReadAsStringAsync();
                if(jwtToken.Result != "User not found")
                {
                    await _customAuthenticationStateProvider.SetTokenAsync(jwtToken.Result);
                    var identity = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
                    var claims = identity.User.Identities.First().Claims.ToList();
                    if (claims[2].Value == "Employee")
                        _navigationManager.NavigateTo("/Employee");
                    else
                        _navigationManager.NavigateTo("/Admin");
                }
                else
                {
                    LoginPageBase.message = "Incorrect username or password!";
                    _navigationManager.NavigateTo("/");
                }
            }
        }
    }
}
