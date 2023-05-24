using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class MedicalCertificatePageBase : ComponentBase
	{
		[Parameter]
		public string sickLeaveId { get; set; }
		[Inject]
		protected SickLeaveService _sickLeaveService { get; set; }
		[Inject]
		protected NavigationManager _navigationManager { get; set; }
		protected List<MedicalCertificate> medicalCertificates { get; set; }

		protected override async Task OnInitializedAsync()
		{
			medicalCertificates = await _sickLeaveService.GetMedicalCertificatesAsync(int.Parse(sickLeaveId));
		}

		protected void Close()
		{
			_navigationManager.NavigateTo("/Employee");
		}
	}
}
