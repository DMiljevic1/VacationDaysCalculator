using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using VacationDaysCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VacationDaysCalculatorBlazorServer.AutentificationProvider;
using System.IdentityModel.Tokens.Jwt;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class LoginPageBase : ComponentBase
    {
        protected String message {get;set;}
        public UserLogin userLogin { get; set; }

        public System.Security.Claims.ClaimsPrincipal user { get; set; }

        [Inject]
        protected LogInService _logInService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        ProtectedLocalStorage _browserStorage { get; set; }

        [CascadingParameter] 
        public Task<AuthenticationState> _authTask { get; set; }

        [Inject] 
        private AuthenticationStateProvider _authState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userLogin = new UserLogin();
            user = new System.Security.Claims.ClaimsPrincipal();
            message = "";
        }

        protected async void AuthorizeLogin()
        {
               var token = await _logInService.SendUserAsync(userLogin);
                await _browserStorage.SetAsync("name", token);
            if (token != null)
            {
                var authState = await ((CustomAuthenticationStateProvider)_authState).ChangeUser("dmiljevic", "1", "Employee");
                this.user = authState.User;
            }
            else
            {
                _navigationManager.NavigateTo("/LoginPage");
                message = "Invalid password or username";
            }
        }
    }
}
