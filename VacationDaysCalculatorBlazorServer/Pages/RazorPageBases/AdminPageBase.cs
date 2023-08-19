using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AdminPageBase : ComponentBase
    {
        protected string searchString1 = "";

        protected Vacation selectedVacation { get; set; }
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }

        [Inject]
        protected SickLeaveService _sickLeaveService { get; set; }
        protected ConfirmationDialog CancelConfirmationDialog { get; set; }
        protected AdminDetails adminDetails { get; set; }
        protected string vacationOrSickLeave { get; set; }
        protected List<SickLeave> closedSickLeaves { get; set; }

        protected IEnumerable<Vacation> vacations = new List<Vacation>();
        protected override async Task OnInitializedAsync()
        {
            adminDetails = await _adminService.GetAdminDetailsAsync();
            if(adminDetails != null)
                vacations = adminDetails.EmployeeVacationDays;
            closedSickLeaves = await _sickLeaveService.GetClosedSickLeavesAsync();
        }
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
			if (ConvertDateToString(sickLeave.SickLeaveTo).Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			return false;
		}
		private string ConvertDateToString(Object obj)
		{
			if (obj == null)
				return "";
			DateTime date = (DateTime)obj;
			return date.ToString("dd.MM.yyyy");
		}
		protected async Task ApproveVacationAsync(Vacation vacation)
        {
            vacation.Status = VacationStatus.Approved;
            vacation.ApprovedBy = adminDetails.LastName + " " + adminDetails.FirstName;
            await _employeeService.UpdateVacationStatusAsync(vacation);
            adminDetails = await _adminService.GetAdminDetailsAsync();
            vacations = adminDetails.EmployeeVacationDays;
        }
        protected async Task DeclineVacationAsync(Vacation vacation)
        {
            selectedVacation = vacation;
            CancelConfirmationDialog.Show();
        }
        protected async Task OpenAddUserPage()
        {
            _navigationManager.NavigateTo("/AddUser");
        }
        protected async Task OpenApprovedVacations()
        {
            _navigationManager.NavigateTo("/ApprovedVacations");
        }
        protected async Task OnDeclineConfirmationSelected(bool isDeclineConfirmed)
        {
            if (isDeclineConfirmed)
            {
                selectedVacation.Status = VacationStatus.Declined;
                await _employeeService.UpdateVacationStatusAsync(selectedVacation);
                adminDetails = await _adminService.GetAdminDetailsAsync();
                vacations = adminDetails.EmployeeVacationDays;
            }
        }

        protected void OpenSickLeaveList()
        {
            _navigationManager.NavigateTo("/SickLeaveList");
        }

		protected void OpenMedicalCertificatePage(int sickLeaveId)
		{
			_navigationManager.NavigateTo("/MedicalCertificate/" + sickLeaveId);
		}

		protected async Task ArchiveSickLeave(SickLeave sickLeave)
        {
            await _sickLeaveService.ArchiveSickLeaveAsync(sickLeave);
            closedSickLeaves = await _sickLeaveService.GetSickLeavesAsync();
        }

    }
}
