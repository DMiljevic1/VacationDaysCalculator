using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Inject]
        protected UserService _userService { get; set; }
        protected User currentUser { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentUser = await _userService.GetUserAsync(1);
        }
    }
}
