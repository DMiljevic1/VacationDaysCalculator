using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using VacationDaysCalculatorBlazorServer.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class LoginPageBase : ComponentBase
    {
        public UserLogin userLogin { get; set; }

        protected List<User> Users { get; set; }

        [Inject]
        protected LogInService _logInService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        ProtectedLocalStorage _browserStorage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            userLogin = new UserLogin();
        }

        protected async void AuthorizeLogin()
        {
               var token = await _logInService.SendUserAsync(userLogin);
                await _browserStorage.SetAsync("name", token);
                _navigationManager.NavigateTo("/");
        }
    }
}
