using DomainModel.DtoModels;
using Microsoft.AspNetCore.Components;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class ChangePasswordBase : ComponentBase
	{
		public Password password { get; set; }
        protected override async Task OnInitializedAsync()
        {
            password = new Password();
        }
        protected async Task ChangePasswordAsync()
        {

        }
    }
}
