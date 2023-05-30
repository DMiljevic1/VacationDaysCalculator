using DomainModel.Enums;
using DomainModel.Models;
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
		public List<SickLeave> GetSickLeaveByUserId(int userId)
		{
			return _vacationDbContext.SickLeave.Where(s => s.UserId == userId && (s.SickLeaveStatus == SickLeaveStatus.Opened || s.SickLeaveStatus == SickLeaveStatus.Closed)).ToList();
		}
		public SickLeave GetSickLeaveById(int sickLeaveId)
		{
			return _vacationDbContext.SickLeave.FirstOrDefault(sl => sl.Id == sickLeaveId);
		}
		public void UpdateSickLeave(SickLeave sickLeave)
		{
			var sickLeaveForUpdate = GetSickLeaveById(sickLeave.Id);
			if (sickLeaveForUpdate != null)
			{
				if (sickLeave.SickLeaveStatus == SickLeaveStatus.Closed)
					sickLeaveForUpdate.SickLeaveTo = sickLeave.SickLeaveTo;
				sickLeaveForUpdate.SickLeaveStatus = sickLeave.SickLeaveStatus;
				_vacationDbContext.SaveChanges();
			}
		}
		
		public List<MedicalCertificate> GetMedicalCertificates(int sickLeaveId)
		{
			return _vacationDbContext.MedicalCertificates.Where(s => s.SickLeaveId == sickLeaveId).ToList();
		}

		public void AddSickLeave(SickLeave sickLeave)
		{
			_vacationDbContext.SickLeave.Add(sickLeave);
			_vacationDbContext.SaveChanges();
		}
	}
}
