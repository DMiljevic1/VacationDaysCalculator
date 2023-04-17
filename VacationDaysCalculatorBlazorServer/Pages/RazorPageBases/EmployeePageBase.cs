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
        protected List<Vacation> approvedAndPendingVacationRequests { get; set; }
        protected string vacationOrSickLeave { get; set; }
        protected override async Task OnInitializedAsync()
        {
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync();
            if(currentEmployee != null)
                approvedAndPendingVacationRequests = currentEmployee.VacationDays.Where(v => v.Status == VacationStatus.Pending || v.Status == VacationStatus.Approved).ToList();
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
            approvedAndPendingVacationRequests = currentEmployee.VacationDays.Where(v => v.Status == VacationStatus.Pending || v.Status == VacationStatus.Approved).ToList();
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
        protected void OpenErrorDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters.Add("ContentText", "You don't have enough vacation for that period.");
            _dialogService.Show<ErrorDialog>("Error",parameters, options);
        }
        protected async Task AddVacation(Vacation newVacation)
        {
            var vacationList = new List<DateTime>();
            vacationList.Add(newVacation.VacationFrom);
            vacationList.Add(newVacation.VacationTo);
            int daysInVacationRequest = await _employeeService.CalculateTotalVacationForGivenPeriodAsync(vacationList);
            if (daysInVacationRequest <= currentEmployee.RemainingDaysOffCurrentYear + currentEmployee.RemainingDaysOffLastYear)
            {
                newVacation.Status = VacationStatus.Pending;
                newVacation.VacationRequestDate = DateTime.Now;
                await _employeeService.AddVacationAsync(newVacation);
                _navigationManager.NavigateTo("/Employee", true);
            }
            else
            {
                OpenErrorDialog();
            }
        }
        protected bool isCurrentUserOnVacation()
        {
            if(currentEmployee != null && currentEmployee.VacationDays != null && currentEmployee.VacationDays.Count > 0)
            {
                foreach(var vacationRequest in currentEmployee.VacationDays)
                {
                    if (vacationRequest.Status == VacationStatus.OnVacation)
                        return true;
                }
            }
            return false;
        }
    }
}
