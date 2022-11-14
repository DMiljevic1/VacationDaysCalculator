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
        public UserLogin userLogin { get; set; }

        [Inject]
        protected LogInService _logInService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userLogin = new UserLogin();
        }

        protected async void AuthorizeLogin()
        {
                await _logInService.SendUserAsync(userLogin);
            //    await _browserStorage.SetAsync("name", token);
            //if (token != null)
            //{
            //    var authState = await ((CustomAuthenticationStateProvider)_authState).ChangeUser("token");
            //    this.user = authState.User;
            //}
            //else
            //{
            //    _navigationManager.NavigateTo("/LoginPage");
            //    message = "Invalid password or username";
            //}
        }
    }
}
