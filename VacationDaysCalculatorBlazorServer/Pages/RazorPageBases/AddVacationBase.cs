using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AddVacationBase : ComponentBase
    {
        [Parameter]
        public string userId { get; set; }
        public VacationDays vacation { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public UserService _userService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            vacation = new VacationDays();
        }
        protected void Close()
        {
            _navigationManager.NavigateTo("/Employee/" + userId);
        }
        protected async Task AddVacation()
        {
            vacation.UserId = int.Parse(userId);
            var dateFrom = vacation.VacationFrom;
            vacation.Year = dateFrom.Year;
            vacation.Status = VacationStatus.Pending;
            await _userService.AddVacationAsync(vacation);
            Close();
        }
    }
}
