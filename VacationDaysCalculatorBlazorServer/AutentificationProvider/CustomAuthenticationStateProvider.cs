using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using VacationDaysCalculatorBlazorServer.Services;
using Blazored.LocalStorage;

namespace VacationDaysCalculatorBlazorServer.AutentificationProvider
{
    //ova klasa se poziva svaki put kad se refresha stranica
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ISyncLocalStorageService _syncLocalStorageService;

        //public CustomAuthenticationStateProvider()
        //{
        //    this.CurrentUser = this.GetAnonymous();
        //}

        //private ClaimsPrincipal CurrentUser { get; set; }


        //private ClaimsPrincipal GetUser(string userName, string id, string role)
        //{

        //    var identity = new ClaimsIdentity(new[]
        //    {
        //            new Claim(ClaimTypes. Sid, id),
        //            new Claim(ClaimTypes.Name, userName),
        //            new Claim(ClaimTypes.Role, role)
        //        }, "Authentication type");
        //    return new ClaimsPrincipal(identity);
        //}


        //private ClaimsPrincipal GetAnonymous()
        //{
        //    var identity = new ClaimsIdentity(new[]
        //   {
        //            new Claim(ClaimTypes.Sid, "0"),
        //            new Claim(ClaimTypes.Name, "Anonymous"),
        //            new Claim(ClaimTypes.Role, "Anonymous")
        //        }, null);

        //    return new ClaimsPrincipal(identity);
        //}


        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var task = Task.FromResult(new AuthenticationState(this.CurrentUser));// tu ce umisto current usera ic getItem di cu dobit usera

            return task;
        }

        public Task<AuthenticationState> ChangeUser()
        {

            var parseClaims = new TokenDecoder();
            var claims = parseClaims.ParseClaimsFromJwt(_syncLocalStorageService.GetItemAsString("token"));
            var task = this.GetAuthenticationStateAsync();
            this.NotifyAuthenticationStateChanged(task);
            return task;
        }
    }
}