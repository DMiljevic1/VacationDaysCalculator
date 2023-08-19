using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
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

		public MedicalCertificate GetMedicalCertificate(int medicalCertificateId)
		{
			return _vacationDbContext.MedicalCertificates.FirstOrDefault(m => m.Id == medicalCertificateId);
		}

		public void UploadMedicalCertificateFile(MedicalCertificate medicalCertificate)
		{
			var medicalCertificateForUpdate = GetMedicalCertificate(medicalCertificate.Id);
			if (medicalCertificate != null && medicalCertificate.Attachment != null && medicalCertificate.Attachment.Length > 0)
			{
				medicalCertificateForUpdate.Attachment = medicalCertificate.Attachment;
				medicalCertificateForUpdate.FileSize = medicalCertificate.FileSize;
				medicalCertificateForUpdate.FileName = medicalCertificate.FileName;
				_vacationDbContext.SaveChanges();
			}
		}

		public void AddMedicalCertificate(MedicalCertificate medicalCertificate)
		{
            _vacationDbContext.MedicalCertificates.Add(medicalCertificate);
            _vacationDbContext.SaveChanges();
        }

		public List<SickLeave> GetOpenedSickLeaves()
		{
			return _vacationDbContext.SickLeave.Where(s => s.SickLeaveStatus == SickLeaveStatus.Opened).ToList();
		}

		public List<SickLeave> GetSickLeavesByUserIdAndStatus(int userId, SickLeaveStatus status)
		{
			return _vacationDbContext.SickLeave.Include(sl => sl.User).Where(sl => sl.UserId == userId && sl.SickLeaveStatus == status).ToList();
		}

		public List<SickLeave> GetSickLeavesByStatus(SickLeaveStatus status)
		{
            return _vacationDbContext.SickLeave.Include(sl => sl.User).Where(s => s.SickLeaveStatus == status).ToList();
        }

		public List<SickLeave> GetSickLeaves()
		{
			return _vacationDbContext.SickLeave.Include(sl => sl.User).ToList();
		}

		public void UpdateSickLeaveStatus(int sickLeaveId, SickLeaveStatus status)
		{
			var sickLeaveForUpdate = GetSickLeaveById(sickLeaveId);
			if(sickLeaveForUpdate != null)
			{
				sickLeaveForUpdate.SickLeaveStatus = status;
				_vacationDbContext.SaveChanges();
			}
		}
	}
}
