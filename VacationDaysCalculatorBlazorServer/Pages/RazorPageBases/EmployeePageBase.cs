using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using DomainModel.Models;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Parameter]
        public string employeeId { get; set; }
        [Inject]
        protected UserService _userService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected EmployeeDetails currentEmployee { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _userService.GetEmployeeDetailsAsync(int.Parse(employeeId));
        }
        protected void OpenEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/EmployeeHistory/" + employeeId);
        }
        protected void OpenAddVacationPage()
        {
            _navigationManager.NavigateTo("/AddVacation/" + employeeId);
        }
    }
}
