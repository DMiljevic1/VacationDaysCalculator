using DomainModel.Enums;
using DomainModel.Models;
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
		public void CloseSickLeave(SickLeave sickLeave)
		{
			sickLeave.SickLeaveStatus = SickLeaveStatus.Closed;
			sickLeave.SickLeaveTo = DateTime.Today;
			_sickLeaveRepository.UpdateSickLeave(sickLeave);
		}
	}
}
