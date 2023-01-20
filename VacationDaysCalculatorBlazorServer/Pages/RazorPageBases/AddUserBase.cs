using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using System.Text;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;
using VacationDaysCalculatorBlazorServer.ValidationModels;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class AddUserBase : ComponentBase
	{
        private int MAX_VACATION_DAYS_PER_YEAR = 20;
        public UserDetails userDetails { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public AdminService _adminService { get; set; }
        protected NotificationDialog NotificationDialog { get; set; }
        protected List<ValidationError> ValidationErrors { get; set; }
        protected String ConcatenatedValidationErrors { get; set; }
        protected override async Task OnInitializedAsync()
        {
            userDetails = new UserDetails();
        }
        protected void Close()
        {
            _navigationManager.NavigateTo("/Admin");
        }
        protected async Task AddUserAsync()
        {
            ValidationErrors = ValidateUser();
            if (ValidationErrors.Any())
            {
                ConcatenatedValidationErrors = GetConcatenatedValidationErrors(ValidationErrors);
                NotificationDialog.Show();
            }
            else
            {
                await _adminService.AddUserAsync(userDetails);
                Close();
            }
        }
        private List<ValidationError> ValidateUser()
        {
            var validationErrors = new List<ValidationError>();

            if (String.IsNullOrWhiteSpace(userDetails.Username))
                validationErrors.Add(new ValidationError { Description = "Please insert username!" });

            if (String.IsNullOrWhiteSpace(userDetails.Password))
                validationErrors.Add(new ValidationError { Description = "Please insert password!" });

            if (userDetails.Password != userDetails.ConfirmPassword)
                validationErrors.Add(new ValidationError { Description = "Passwords must match!" });

            if (String.IsNullOrWhiteSpace(userDetails.FirstName))
                validationErrors.Add(new ValidationError { Description = "Please insert first name!" });

            if (String.IsNullOrWhiteSpace(userDetails.LastName))
                validationErrors.Add(new ValidationError { Description = "Please insert last name!" });

            if (userDetails.Role == null)
                validationErrors.Add(new ValidationError { Description = "Please choose user's role!" });

            if(userDetails.RemainingDaysOffCurrentYear > MAX_VACATION_DAYS_PER_YEAR && userDetails.Role == "Employee")
                validationErrors.Add(new ValidationError { Description = "User can have max " + MAX_VACATION_DAYS_PER_YEAR + " days of vacation per year!" });

            if(userDetails.RemainingDaysOffCurrentYear < MAX_VACATION_DAYS_PER_YEAR && userDetails.RemainingDaysOffLastYear != 0 && userDetails.Role == "Employee")
                validationErrors.Add(new ValidationError { Description = "If vacation from current year is less than " + MAX_VACATION_DAYS_PER_YEAR + ", vacation from last year must be 0!" });

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
