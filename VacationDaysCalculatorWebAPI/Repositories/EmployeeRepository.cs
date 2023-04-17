using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class EmployeeRepository
    {
        private readonly VacationDbContext _vacationDbContext;
        public EmployeeRepository(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }
        public List<User> GetUsers()
        {
            return _vacationDbContext.Users.ToList();
        }
        public User GetUserById(int userId)
        {
            return _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        }
        public List<Vacation> GetVacationRequestsWithPendingOrApprovedStatus(int userId)
        {
            return _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && (vd.Status.Equals(VacationStatus.Approved) || vd.Status.Equals(VacationStatus.OnVacation) || vd.Status.Equals(VacationStatus.Pending))).ToList();
        }
        public List<Vacation> GetVacationRequests()
        {
            return _vacationDbContext.Vacation.ToList();
        }
        public Vacation GetVacationByVacationId(int vacationId)
        {
            return _vacationDbContext.Vacation.FirstOrDefault(vd => vd.Id == vacationId);
        }
        public List<Vacation> GetArchivedVacations(int userId)
        {
            return _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && vd.Status.Equals(VacationStatus.Arhived)).ToList();
        }
        public void AddVacation(Vacation vacation)
        {
            _vacationDbContext.Vacation.Add(vacation);
            _vacationDbContext.SaveChanges();
        }
        public void RemoveVacation(Vacation vacation)
        {
            _vacationDbContext.Vacation.Remove(vacation);
            _vacationDbContext.SaveChanges();
        }
        public void UpdateEmployeeRemainingVacation(User user)
        {
            var employeeForUpdate = GetUserById(user.Id);
            employeeForUpdate.RemainingDaysOffLastYear = user.RemainingDaysOffLastYear;
            employeeForUpdate.RemainingDaysOffCurrentYear = user.RemainingDaysOffCurrentYear;

            _vacationDbContext.SaveChanges();
        }
        public void UpdateEmployeeVacationStatus(Vacation vacation)
        {
            var vacationForUpdate = GetVacationByVacationId(vacation.Id);
            if (vacationForUpdate != null)
            {
                if(vacation.Status == VacationStatus.Approved)
                    vacationForUpdate.ApprovedBy = vacation.ApprovedBy;

                vacationForUpdate.Status = vacation.Status;

                _vacationDbContext.SaveChanges();
            }
        }
        public void SetRemainingVacationOnFirstDayOfYear(int totalGivenVacationPerYear)
        {
            var currentYear = DateTime.Now.Year;
            var users = GetUsers();
            foreach (var user in users)
            {
                user.CurrentYear = currentYear;
                user.RemainingDaysOffLastYear = user.RemainingDaysOffCurrentYear;
                user.RemainingDaysOffCurrentYear = totalGivenVacationPerYear;
            }
            _vacationDbContext.SaveChanges();
        }
		public SickLeave GetSickLeaveByUserId(int userId)
		{
            return _vacationDbContext.SickLeave.FirstOrDefault(s => s.IsClosed == false && s.UserId == userId);
		}
	}
}
