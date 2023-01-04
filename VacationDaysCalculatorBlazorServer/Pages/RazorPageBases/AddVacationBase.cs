using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;
using VacationDaysCalculatorBlazorServer.ValidationModels;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class AddVacationBase : ComponentBase
    {
        public Vacation vacation { get; set; }
        public EmployeeDetails currentEmployee { get; set; }
        public int vacationPlannedToSpent { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public EmployeeService _employeeService { get; set; }
        [Inject]
        public EmployeeService _userService { get; set; }
        protected NotificationDialog NotificationDialog { get; set; }
        protected List<ValidationError> ValidationErrors { get; set; }
        protected String ConcatenatedValidationErrors { get; set; }
        protected override async Task OnInitializedAsync()
        {
            vacation = new Vacation();
            currentEmployee = await _employeeService.GetEmployeeDetailsAsync();
        }
        protected void Close()
        {
            _navigationManager.NavigateTo("/Employee");
        }
        protected async Task AddVacation()
        {
            var vacationList = new List<DateTime>();
            vacationList.Add(vacation.VacationFrom);
            vacationList.Add(vacation.VacationTo);
            vacationPlannedToSpent = await _employeeService.CalculateTotalVacationForGivenPeriodAsync(vacationList);
            ValidationErrors = ValidateUser();
            if (ValidationErrors.Any())
            {
                ConcatenatedValidationErrors = GetConcatenatedValidationErrors(ValidationErrors);
                NotificationDialog.Show();
            }
            else
            {
                var dateFrom = vacation.VacationFrom;
                vacation.Status = VacationStatus.Pending;
                vacation.VacationRequestDate = DateTime.Now;
                await _userService.AddVacationAsync(vacation);
                Close();
            }
        }
        private List<ValidationError> ValidateUser()
        {
            int MAX_NUMBER_OF_MONTHS = 5;
            var validationErrors = new List<ValidationError>();
            var currentDate = DateTime.Now.Date;
            var maxDateToTakeVacation = currentDate.Date.AddMonths(MAX_NUMBER_OF_MONTHS);

            if (vacation.VacationFrom.Date.CompareTo(currentDate) < 0)
                validationErrors.Add(new ValidationError { Description = "Vacation From cannot be before current date!" });

            if (vacation.VacationTo.Date.CompareTo(vacation.VacationFrom.Date) < 0)
                validationErrors.Add(new ValidationError { Description = "Vacation To cannot be before Vacation From!" });

            if (vacation.VacationFrom.Date.CompareTo(maxDateToTakeVacation) > 0)
                validationErrors.Add(new ValidationError { Description = "You cannot take vacation for more then " + MAX_NUMBER_OF_MONTHS + " months in advance!" });

            if (currentEmployee.RemainingDaysOffLastYear + currentEmployee.RemainingDaysOffCurrentYear < vacationPlannedToSpent)
                validationErrors.Add(new ValidationError { Description = "You don't have enough vacation for that period!" });

            return validationErrors;
        }
        private string GetConcatenatedValidationErrors(List<ValidationError> ValidationErrors)
        {
            StringBuilder message = new StringBuilder();
            foreach (var error in ValidationErrors)
            {
                if (message.Length == 0)
                    message.Append(error.Description);
                else
                    message.Append($"{Environment.NewLine} {error.Description}");

            }
            return message.ToString();
        }
    }
}
