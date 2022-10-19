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

        protected async void AuthorizeLogin()
        {
            //if (userLogin.UserName || userLogin.Password.Equals(null))
            //{
            //    Console.WriteLine("krivi podaci");
            //    //ne ulazi, trebalo bi neki notification dialog da je kriva lozinka
            //}
            //else
            //{
                await LogInService.SendUserAsync(userLogin);
                _navigationManager.NavigateTo("/");
            //}
        }
    }
}
