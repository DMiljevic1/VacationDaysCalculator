using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using DomainModel.Models;
using VacationDaysCalculatorBlazorServer.Services;
using DomainModel.Enums;
using MudBlazor;
using VacationDaysCalculatorBlazorServer.ValidationModels;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeePageBase : ComponentBase
    {
        [Inject]
        protected IDialogService _dialogService { get; set; }
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
        protected void OpenAddVacationDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            _dialogService.Show<AddVacationDialog>("Send Vacation Request", options);
        }
        protected async Task AddVacation(Vacation newVacation)
        {
            var vacationList = new List<DateTime>();
            vacationList.Add(newVacation.VacationFrom);
            vacationList.Add(newVacation.VacationTo);
            newVacation.Status = VacationStatus.Pending;
            newVacation.VacationRequestDate = DateTime.Now;
            await _employeeService.AddVacationAsync(newVacation);
            _navigationManager.NavigateTo("/Employee", true);
        }
    }
}
