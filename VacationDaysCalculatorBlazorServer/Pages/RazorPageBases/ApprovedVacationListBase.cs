using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class ApprovedVacationListBase : ComponentBase
    {
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected List<Vacation> approvedVacations { get; set; }
        protected override async Task OnInitializedAsync()
        {
            approvedVacations = await _adminService.GetApprovedVacations();
        }
        protected void CloseApprovedVacationsPage()
        {
            _navigationManager.NavigateTo("/Admin");
        }
    }
}
