using DomainModel.DtoModels;
using DomainModel.Models;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
	public class HolidayRepository
	{
		private readonly VacationDbContext _vacationDbContext;
		public HolidayRepository(VacationDbContext vacationDbContext)
		{
			_vacationDbContext = vacationDbContext;
		}
		public List<Holiday> GetHolidaysForCurrentAndNextYear()
		{
			int currentYear = DateTime.Now.Year;
			int nextYear = currentYear + 1;
			return _vacationDbContext.Holidays.Where(h => h.Year.Equals(currentYear) || h.Year.Equals(nextYear)).ToList();
		}
		public void AddHolidays(List<Holiday> holidays)
		{
			foreach (var holiday in holidays)
			{
				_vacationDbContext.Add(holiday);
			}
			_vacationDbContext.SaveChanges();
		}
		public List<Holiday> GetHolidays()
		{
			return _vacationDbContext.Holidays.ToList();
		}
		public void DeleteHoliday(int holidayId)
		{
			var holidayForDelete = GetHolidayById(holidayId);
			if (holidayForDelete != null)
				_vacationDbContext.Holidays.Remove(holidayForDelete);
			_vacationDbContext.SaveChanges();
		}
		public Holiday GetHolidayById(int holidayId)
		{
			return _vacationDbContext.Holidays.FirstOrDefault(h => h.Id == holidayId);
		}
		public void UpdateHoliday(Holiday holiday)
		{
			var holidayForUpdate = GetHolidayById(holiday.Id);
			holidayForUpdate.HolidayName = holiday.HolidayName;
			holidayForUpdate.HolidayDate = holiday.HolidayDate;
			holidayForUpdate.Year = holiday.Year;
			
			_vacationDbContext.SaveChanges();
		}
		public void AddHoliday(Holiday holiday)
		{
			_vacationDbContext.Add(holiday);
			_vacationDbContext.SaveChanges();
		}
	}
}
