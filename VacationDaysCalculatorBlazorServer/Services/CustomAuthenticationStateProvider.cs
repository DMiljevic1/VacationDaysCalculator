using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace VacationDaysCalculatorBlazorServer.Services
{
    public class CustomAuthenticationStateProvider : ServerAuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetTokenAsync()
        {

            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

        }

        public async Task SetTokenAsync(string token)
        {
            if (token == null)
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.removeItem", "authToken");
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("localStorage.setItem", "authToken", token);
            }

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync();
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(ServiceExtensions.ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public async Task<int> GetUserId()
        {
            var identity = await GetAuthenticationStateAsync();
            var claims = identity.User.Identities.First().Claims.ToList();
            if(claims.Any())
                return int.Parse(claims[0].Value);
            return 0;
        }
    }
}
