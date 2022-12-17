using DomainModel.DtoModels;
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
    }
}
