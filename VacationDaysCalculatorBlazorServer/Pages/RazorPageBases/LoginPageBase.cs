using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using System;
using System.Threading.Tasks;


namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class LoginPageBase : ComponentBase
    {
        public string username, password;

        protected List<User> Users { get; set; }

        [Inject]
        protected UserService _userService { get; set; }

        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await _userService.GetUsers();
        }

        protected async void AuthorizeLogin()
        {
            var user = await _userService.GetUserByUserName(username, password);
            if (user == null)
            {

            }
            else
            {
                //implementirat token nekako jos
                _navigationManager.NavigateTo("/");
            }
    }
}
