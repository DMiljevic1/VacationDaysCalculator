using DomainModel.DtoModels;
using DomainModel.Models;
using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Services
{
	public class HolidayService
	{
		private readonly HolidayRepository _holidayRepository;
		public HolidayService(HolidayRepository holidayRepository)
		{
			_holidayRepository = holidayRepository;
		}
		public void AddHolidays(List<HolidayDetails> holidaysDetails)
		{
			List<Holiday> holidays = ConvertHolidayDetailsToHoliday(holidaysDetails);
			_holidayRepository.AddHolidays(holidays);
		}
		private List<Holiday> ConvertHolidayDetailsToHoliday(List<HolidayDetails> holidaysDetails)
		{
			var holidays = new List<Holiday>();
			int currentYear = DateTime.Now.Year;
			foreach (var holidayDetails in holidaysDetails)
			{
				var holiday = new Holiday();
				holiday.Year = currentYear;
				holiday.HolidayDate = holidayDetails.Date;
				holiday.HolidayName = holidayDetails.LocalName;
				holidays.Add(holiday);
			}
			return holidays;
		}
	}
}
