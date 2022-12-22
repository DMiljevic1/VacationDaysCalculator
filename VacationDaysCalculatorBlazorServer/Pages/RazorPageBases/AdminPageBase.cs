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
        [Parameter]
        public string userId { get; set; }
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected AdminDetails adminDetails { get; set; }
        protected override async Task OnInitializedAsync()
        {
            adminDetails = await _adminService.GetAdminDetailsAsync(int.Parse(userId));
        }
        protected async Task ApproveEmployeeVacation(VacationDays vacationDays)
        {
            vacationDays.Status = VacationStatus.Approved;
            await _adminService.UpdateEmployeeVacationStatusAsync(vacationDays);
            adminDetails = await _adminService.GetAdminDetailsAsync(int.Parse(userId));
        }
        protected async Task CancelEmployeeVacation(VacationDays vacationDays)
        {
            vacationDays.Status = VacationStatus.Cancelled;
            await _adminService.UpdateEmployeeVacationStatusAsync(vacationDays);
            adminDetails = await _adminService.GetAdminDetailsAsync(int.Parse(userId));
        }
    }
}
