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
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected EmployeeDetails currentEmployee { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync();
        }
        protected void OpenEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/EmployeeHistory");
        }
        protected void OpenAddVacationPage()
        {
            _navigationManager.NavigateTo("/AddVacation");
        }
        protected async Task DeleteVacationRequest(int vacationId)
        {
            await _employeeService.DeleteVacationRequestAndRestoreVacationAsync(vacationId);
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync();
        }
        //fields and methods for mud table
        protected bool dense = false;
        protected bool hover = true;
        protected bool striped = false;
        protected bool bordered = false;
        protected string searchString1 = "";
        protected Vacation selectedVacation { get; set; }
        public bool FilterFunction(Vacation vacationRequest) => FilterFunc(vacationRequest, searchString1);

        private bool FilterFunc(Vacation vacationRequest, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (vacationRequest.User.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (vacationRequest.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (vacationRequest.VacationRequestDate.ToString("dd.MM.yyyy.hh:mm").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (vacationRequest.VacationFrom.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (vacationRequest.VacationTo.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        //end
    }
}
