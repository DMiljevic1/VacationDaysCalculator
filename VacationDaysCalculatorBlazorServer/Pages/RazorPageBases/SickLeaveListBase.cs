using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
    public class SickLeaveListBase : ComponentBase
    {
		[Inject]
		protected SickLeaveService _sickLeaveService { get; set; }
		[Inject]
		protected NavigationManager _navigationManager { get; set; }
		protected List<SickLeave> sickLeaveList { get; set; }
		protected override async Task OnInitializedAsync()
		{
			sickLeaveList = await _sickLeaveService.GetSickLeavesAsync();
		}
		protected void Close()
		{
			_navigationManager.NavigateTo("/Admin");
		}
		protected string searchString1 = "";
		public bool FilterFunction(SickLeave sickLeave) => FilterFunc(sickLeave, searchString1);

		private bool FilterFunc(SickLeave sickLeave, string searchString)
		{
			if (string.IsNullOrWhiteSpace(searchString))
				return true;
			if (sickLeave.User.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (sickLeave.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (sickLeave.SickLeaveFrom.ToString("dd.MM.yyyy.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (ConvertDateToString(sickLeave.SickLeaveTo).Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (sickLeave.SickLeaveStatus.ToString().Contains(searchString))
				return true;
			return false;
		}
		private string ConvertDateToString(Object obj)
		{
			if (obj == null)
				return "";
			DateTime date = (DateTime)obj;
			return date.ToString("dd.MM.yyyy");
		}
		protected void OpenMedicalCertificatePage(int sickLeaveId)
		{
			_navigationManager.NavigateTo("/MedicalCertificate/" + sickLeaveId);
		}
	}
}
