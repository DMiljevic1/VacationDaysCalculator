using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class ChangePasswordBase : ComponentBase
	{
		public Password password { get; set; }
        public string currentPassword { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public CommonService _commonService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            password = new Password();
            currentPassword = await _commonService.GetUserPassword();
        }
        protected async Task ChangePasswordAsync()
        {
            await _commonService.ChangePasswordAsync(password);
            _navigationManager.NavigateTo("/ChangePassword");
        }
    }
}
