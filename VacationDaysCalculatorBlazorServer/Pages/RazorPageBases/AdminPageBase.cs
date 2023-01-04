using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AdminPageBase : ComponentBase
    {
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected EmployeeService _employeeService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
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
            vacation.Status = VacationStatus.Cancelled;
            await _employeeService.CancelVacationAsync(vacation);
            adminDetails = await _adminService.GetAdminDetailsAsync();
        }
        protected async Task OpenAddUserPage()
        {
            _navigationManager.NavigateTo("/AddUser");
        }
        protected async Task OpenApprovedVacations()
        {
            _navigationManager.NavigateTo("/ApprovedVacations");
        }
    }
}
