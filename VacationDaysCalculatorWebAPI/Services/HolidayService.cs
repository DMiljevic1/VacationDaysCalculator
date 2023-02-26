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
			foreach (var holidayDetails in holidaysDetails)
			{
				var holiday = new Holiday();
				holiday.Year = holidayDetails.Date.Year;
				holiday.HolidayDate = holidayDetails.Date;
				holiday.HolidayName = holidayDetails.LocalName;
				holidays.Add(holiday);
			}
			return holidays;
		}
		public void AddHoliday(Holiday holiday)
		{
			if(holiday != null && holiday.HolidayDate != null && holiday.HolidayName != "")
				_holidayRepository.AddHoliday(holiday);
		}
		public void UpdateHoliday(Holiday holiday)
		{
			if (holiday != null && holiday.HolidayDate != null && holiday.HolidayName != "")
				_holidayRepository.UpdateHoliday(holiday);
		}
	}
}
