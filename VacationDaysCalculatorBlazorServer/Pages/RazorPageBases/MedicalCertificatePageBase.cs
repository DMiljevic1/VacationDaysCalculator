using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Text;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class MedicalCertificatePageBase : ComponentBase
	{
		[Parameter]
		public string sickLeaveId { get; set; }
		[Inject]
		protected IJSRuntime _jsruntime { get; set; }
		[Inject]
		protected SickLeaveService _sickLeaveService { get; set; }
		[Inject]
		protected NavigationManager _navigationManager { get; set; }
		protected List<MedicalCertificate> medicalCertificates { get; set; }

		protected IList<IBrowserFile> files = new List<IBrowserFile>();

		protected override async Task OnInitializedAsync()
		{
			medicalCertificates = await _sickLeaveService.GetMedicalCertificatesAsync(int.Parse(sickLeaveId));
		}

		protected void Close()
		{
			_navigationManager.NavigateTo("/Employee");
		}

		protected async Task UploadMedicalCertificate(InputFileChangeEventArgs e, MedicalCertificate medicalCertificate)
		{
			string file = string.Join(", ", e.GetMultipleFiles().Select(f => f.Name));
			medicalCertificate.Attachment = Encoding.ASCII.GetBytes(file);
			await _sickLeaveService.UploadMedicialCertificate(medicalCertificate);
		}

		protected async Task DownloadMedicalCertificate(MedicalCertificate medicalCertificate)
		{
			await _jsruntime.InvokeVoidAsync("download", "Proba1", medicalCertificate.Attachment);
			medicalCertificates = await _sickLeaveService.GetMedicalCertificatesAsync(int.Parse(sickLeaveId));
		}
	}
}
