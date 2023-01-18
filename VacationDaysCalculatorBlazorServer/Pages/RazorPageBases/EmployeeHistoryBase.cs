using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeeHistoryBase : ComponentBase
    {
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected List<EmployeeHistory> employeeHistory { get; set; }
        protected override async Task OnInitializedAsync()
        {
            employeeHistory = await _employeeService.GetEmployeeHistoryAsync();
        }
        protected void CloseEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/Employee");
        }
        protected string searchString1 = "";
        protected EmployeeHistory selectedHistory { get; set; }
        public bool FilterFunction(EmployeeHistory history) => FilterFunc(history, searchString1);

        private bool FilterFunc(EmployeeHistory history, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (history.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (history.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (history.VacationRequestDate.ToString("dd.MM.yyyy.hh:mm").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (history.VacationFrom.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (history.VacationTo.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (history.ApprovedBy.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
    }
}
