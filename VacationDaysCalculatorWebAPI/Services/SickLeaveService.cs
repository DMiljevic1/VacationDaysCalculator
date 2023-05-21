using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Services
{
	public class SickLeaveService
	{
		private readonly SickLeaveRepository _sickLeaveRepository;
		public SickLeaveService(SickLeaveRepository sickLeaveRepository)
		{
			_sickLeaveRepository = sickLeaveRepository;
		}
	}
}
