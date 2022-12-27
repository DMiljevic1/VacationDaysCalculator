using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Service;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class AddUserBase : ComponentBase
	{
        public UserDetails userDetails { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }
        [Inject]
        public AdminService _adminService { get; set; }
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
            //if(userDetails == null)
            //    return; //dodat validaciju
            //if (userDetails.Password != userDetails.ConfirmPassword)
            //    return; // dodat validaciju
            await _adminService.AddUserAsync(userDetails);
            Close();
        }
    }
}
