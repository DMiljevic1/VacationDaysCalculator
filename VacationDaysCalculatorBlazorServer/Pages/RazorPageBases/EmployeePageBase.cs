using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using DomainModel.Models;
using VacationDaysCalculatorBlazorServer.Services;
using DomainModel.Enums;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected EmployeeDetails currentEmployee { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync(int.Parse(userId));
        }
        protected void OpenEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/EmployeeHistory/" + userId);
        }
        protected void OpenAddVacationPage()
        {
            _navigationManager.NavigateTo("/AddVacation/" + userId);
        }
        protected async Task DeleteVacationRequest(int vacationId)
        {
            await _employeeService.DeleteVacationRequestAndRestoreVacationAsync(vacationId);
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync(int.Parse(userId));
        }
    }
}
