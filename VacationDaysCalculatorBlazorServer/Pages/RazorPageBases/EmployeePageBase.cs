using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        [Inject]
        protected UserService _userService { get; set; }
        protected EmployeeDetails currentEmployee { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _userService.GetEmployeeDetailsAsync(int.Parse(userId));
        }
    }
}
