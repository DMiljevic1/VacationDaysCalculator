using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeeHistoryBase : ComponentBase
    {
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        protected SickLeaveService _sickLeaveService { get; set; }
        protected List<EmployeeHistory> employeeHistory { get; set; }
        protected List<SickLeave> arhivedSickLeaves { get; set; }
        protected override async Task OnInitializedAsync()
        {
            employeeHistory = await _employeeService.GetEmployeeHistoryAsync();
            arhivedSickLeaves = await _sickLeaveService.GetArchivedSickLeavesByUserIdAsync();
        }
        protected void CloseEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/Employee");
        }
        protected string searchString1 = "";
        protected EmployeeHistory selectedHistory { get; set; }
        public bool FilterFunction(EmployeeHistory history) => FilterFunc(history, searchString1);
		protected string vacationOrSickLeave { get; set; }

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
		public bool FilterSickLeaveDelegate(SickLeave sickLeave) => FilterSickLeaveFunction(sickLeave, searchString1);
		private bool FilterSickLeaveFunction(SickLeave sickLeave, string searchString)
		{
			if (string.IsNullOrWhiteSpace(searchString))
				return true;
			if (sickLeave.User.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (sickLeave.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (sickLeave.SickLeaveFrom.ToString("dd.MM.yyyy.hh:mm").Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			return false;
		}
	}
}
