using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class HolidayPageBase : ComponentBase
	{
		[Inject]
		protected IDialogService _dialogService { get; set; }
		[Inject]
		public HolidayService _holidayService { get; set; }
		[Inject]
		public NavigationManager _navigationManager { get; set; }
		protected List<Holiday> holidays { get; set; }
		protected string searchString = "";
		protected override async Task OnInitializedAsync()
		{
			holidays = await _holidayService.GetHolidaysAsync();
		}
		public bool FilterFunction(Holiday holiday) => FilterFunc(holiday, searchString);

		private bool FilterFunc(Holiday holiday, string searchString)
		{
			if (string.IsNullOrWhiteSpace(searchString))
				return true;
			if (holiday.HolidayName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (holiday.Year.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			if (holiday.HolidayDate.ToString("dd.MM.").Contains(searchString, StringComparison.OrdinalIgnoreCase))
				return true;
			return false;
		}
		protected async Task DeleteHolidayAsync(int holidayId)
		{
			await _holidayService.DeleteHolidayAsync(holidayId);
			_navigationManager.NavigateTo("/Holidays", true);
		}
		protected async Task OpenDeleteHolidayConfirmationDialog(int holidayId)
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			var parameters = new DialogParameters();
			parameters.Add("contentText", "Are you sure you want to delete this holiday?");
			parameters.Add("holidayId", holidayId);
			_dialogService.Show<DeleteHolidayConfirmationDialog>("Delete Holiday", parameters, options);
		}
		protected async Task UpdateHolidayAsync(Holiday holiday)
		{
			await _holidayService.UpdateHolidayAsync(holiday);
			holidays = await _holidayService.GetHolidaysAsync();
			_navigationManager.NavigateTo("/Holidays", true);
		}
		protected void OpenEditHolidayDialog(Holiday holiday)
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			var parameters = new DialogParameters();
			parameters.Add("holiday", holiday);
			_dialogService.Show<EditHolidayDialog>("Edit Holiday", parameters, options);
		}
		protected void OpenAddHolidayDialog()
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			_dialogService.Show<AddHolidayDialog>("Add Holiday", options);
		}
		protected async Task AddHolidayAsync(Holiday holiday)
		{
			await _holidayService.AddHolidayAsync(holiday);
			holidays = await _holidayService.GetHolidaysAsync();
			_navigationManager.NavigateTo("/Holidays", true);
		}
	}
}
