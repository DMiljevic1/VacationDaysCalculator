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
        protected async Task UpdateEmployeeVacationStatus(Vacation vacation, VacationStatus vacationStatus)
        {
            vacation.Status = vacationStatus;
            await _employeeService.UpdateEmployeeVacationStatusAsync(vacation);
            adminDetails = await _adminService.GetAdminDetailsAsync();
        }
        protected async Task OpenAddUserPage()
        {
            _navigationManager.NavigateTo("/AddUser");
        }
    }
}
