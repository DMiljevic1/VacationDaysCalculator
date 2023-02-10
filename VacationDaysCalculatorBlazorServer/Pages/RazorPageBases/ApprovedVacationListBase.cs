using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class ApprovedVacationListBase : ComponentBase
    {
        protected Vacation selectedVacation { get; set; }
        [Inject]
        protected AdminService _adminService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        [Inject]
        protected IJSRuntime _jsruntime { get; set; }
        protected List<Vacation> approvedVacations { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //get all approved vacations not just vacations with status approved
            approvedVacations = await _adminService.GetApprovedVacations();
        }
        protected void CloseApprovedVacationsPage()
        {
            _navigationManager.NavigateTo("/Admin");
        }
        protected string searchString1 = "";
        public bool FilterFunction(Vacation approvedVacation) => FilterFunc(approvedVacation, searchString1);

        private bool FilterFunc(Vacation approvedVacation, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (approvedVacation.User.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.VacationRequestDate.ToString("dd.MM.yyyy.hh:mm").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.VacationFrom.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.VacationTo.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.ApprovedBy.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (approvedVacation.Status.ToString().Contains(searchString))
                return true;
            return false;
        }
        protected async Task DownloadFile()
        {
            await _jsruntime.InvokeVoidAsync("Print");
        }
    }
}
