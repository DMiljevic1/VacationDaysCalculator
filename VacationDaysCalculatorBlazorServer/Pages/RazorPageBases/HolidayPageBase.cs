using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.AspNetCore.Components;
using VacationDaysCalculatorBlazorServer.Services;

namespace VacationDaysCalculatorBlazorServer.Pages.RazorPageBases
{
	public class HolidayPageBase : ComponentBase
	{
		[Inject]
		public HolidayService _holidayService { get; set; }
		[Inject]
		public NavigationManager _navigationManager { get; set; }
		protected List<Holiday> holidays { get; set; }
		protected string searchString = "";
		protected Holiday selectedHoliday { get; set; }
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
	}
}
