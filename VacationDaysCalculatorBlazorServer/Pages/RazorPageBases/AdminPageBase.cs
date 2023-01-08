using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AdminPageBase : ComponentBase
    {
        protected Vacation selectedVacation { get; set; }
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected ConfirmationDialog CancelConfirmationDialog { get; set; }
        protected AdminDetails adminDetails { get; set; }
        protected override async Task OnInitializedAsync()
        {
            adminDetails = await _adminService.GetAdminDetailsAsync();
        }
        protected async Task ApproveVacationAsync(Vacation vacation)
        {
            vacation.Status = VacationStatus.Approved;
            vacation.ApprovedBy = adminDetails.LastName + " " + adminDetails.FirstName;
            await _employeeService.ApproveVacationAsync(vacation);
            adminDetails = await _adminService.GetAdminDetailsAsync();
        }
        protected async Task CancelVacationAsync(Vacation vacation)
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
        protected async Task OnCancelConfirmationSelected(bool isCancelConfirmed)
        {
            if (isCancelConfirmed)
            {
                selectedVacation.Status = VacationStatus.Cancelled;
                await _employeeService.CancelVacationAsync(selectedVacation);
                adminDetails = await _adminService.GetAdminDetailsAsync();
            }
        }
    }
}
