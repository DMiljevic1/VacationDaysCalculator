using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
	public class SickLeaveRepository
	{
		private readonly VacationDbContext _vacationDbContext;
		public SickLeaveRepository(VacationDbContext vacationDbContext)
		{
			_vacationDbContext = vacationDbContext;
		}
	}
}
