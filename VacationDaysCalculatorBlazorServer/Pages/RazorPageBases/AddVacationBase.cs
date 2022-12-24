using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AddVacationBase : ComponentBase
    {
        public Vacation vacation { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public EmployeeService _userService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            vacation = new Vacation();
        }
        protected void Close()
        {
            _navigationManager.NavigateTo("/Employee");
        }
        protected async Task AddVacation()
        {
            var dateFrom = vacation.VacationFrom;
            vacation.Year = dateFrom.Year;
            vacation.Status = VacationStatus.Pending;
            await _userService.AddVacationAsync(vacation);
            Close();
        }
    }
}
