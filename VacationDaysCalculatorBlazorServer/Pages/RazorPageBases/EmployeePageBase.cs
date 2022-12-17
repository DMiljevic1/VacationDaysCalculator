using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using DomainModel.Models;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        [Inject]
        protected EmployeeService _userService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected EmployeeDetails currentEmployee { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _userService.GetEmployeeDetailsAsync(int.Parse(userId));
        }
        protected void OpenEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/EmployeeHistory/" + userId);
        }
        protected void OpenAddVacationPage()
        {
            _navigationManager.NavigateTo("/AddVacation/" + userId);
        }
    }
}
