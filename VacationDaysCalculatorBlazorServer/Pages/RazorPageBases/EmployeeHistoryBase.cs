using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class EmployeeHistoryBase : ComponentBase
    {
        [Parameter]
        public string employeeId { get; set; }
        [Inject]
        protected EmployeeService _userService { get; set; }
        [Inject]
        protected NavigationManager _navigationManager { get; set; }
        protected List<EmployeeHistory> employeeHistory { get; set; }
        protected override async Task OnInitializedAsync()
        {
            employeeHistory = await _userService.GetEmployeeHistoryAsync(int.Parse(employeeId));
        }
        protected void CloseEmployeeHistoryPage()
        {
            _navigationManager.NavigateTo("/Employee/" + employeeId);
        }
    }
}
