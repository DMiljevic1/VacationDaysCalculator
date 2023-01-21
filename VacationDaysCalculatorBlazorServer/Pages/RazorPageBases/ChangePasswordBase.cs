using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Cryptography;
using System.Text;
using VacationDaysCalculatorBlazorServer.Services;
using VacationDaysCalculatorBlazorServer.ValidationModels;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class ChangePasswordBase : ComponentBase
	{
		public Password password { get; set; }
        public string userPassword { get; set; }
        protected List<ValidationError> ValidationErrors { get; set; }
        protected String ConcatenatedValidationErrors { get; set; }
        [Inject]
        public IDialogService _dialogService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public CommonService _commonService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            password = new Password();
            userPassword = await _commonService.GetUserPassword();
        }
        protected async Task ChangePasswordAsync()
        {
            ValidationErrors = ValidatePassword();
            if (ValidationErrors.Any())
                OpenErrorDialog();
            else
            {
                await _commonService.ChangePasswordAsync(password);
                _navigationManager.NavigateTo("/ChangePassword", true);
            }
        }
        protected void OpenErrorDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters.Add("ContentText", GetConcatenatedValidationErrors(ValidationErrors));
            _dialogService.Show<ErrorDialog>("Error", parameters, options);
        }
        private List<ValidationError> ValidatePassword()
        {
            var validationErrors = new List<ValidationError>();

            if (String.IsNullOrWhiteSpace(password.NewPassword))
                validationErrors.Add(new ValidationError { Description = "New password can not be empty!" });

            if (userPassword != HashUserPassword(password.OldPassword))
                validationErrors.Add(new ValidationError { Description = "Incorrect old password!" });

            if (password.NewPassword != password.ConfirmPassword)
                validationErrors.Add(new ValidationError { Description = "New password and confirm password must match!" });

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
        private string HashUserPassword(string plainTextPassword)
        {
            if (plainTextPassword == null || plainTextPassword == "")
                return "";
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, plainTextPassword);
            }
            return hashedPassword;
        }
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
