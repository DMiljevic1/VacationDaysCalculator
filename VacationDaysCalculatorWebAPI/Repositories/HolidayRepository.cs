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
		public List<Holiday> GetHolidaysForCurrentYear()
		{
			int currentYear = DateTime.Now.Year;
			return _vacationDbContext.Holidays.Where(h => h.Year.Equals(currentYear)).ToList();
		}
		public void AddHolidays(List<Holiday> holidays)
		{
			foreach (var holiday in holidays)
			{
				_vacationDbContext.Add(holiday);
			}
			_vacationDbContext.SaveChanges();
		}
	}
}
