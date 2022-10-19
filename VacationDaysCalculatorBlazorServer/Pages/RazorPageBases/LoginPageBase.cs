using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using VacationDaysCalculatorBlazorServer.Services;

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

        protected override async Task OnInitializedAsync()
        {
            userLogin = new UserLogin();
        }

        protected async void AuthorizeLogin()
        {
                await _logInService.SendUserAsync(userLogin);
                _navigationManager.NavigateTo("/");
        }
    }
}
