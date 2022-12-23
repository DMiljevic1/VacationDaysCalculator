using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class AdminRepository
    {
        private readonly VacationDbContext _vacationDbContext;

        public AdminRepository(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }

        public AdminDetails GetAdminDetails(int userId)
        {
            var user = _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;
            var adminDetails = new AdminDetails();
            adminDetails.FirstName = user.FirstName;
            adminDetails.LastName = user.LastName;
            adminDetails.Email = user.Email;
            adminDetails.EmployeeVacationDays = GetEmployeeVacation();
            return adminDetails;
        }
        public List<Vacation> GetEmployeeVacation()
        {
            var employeeVacation = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.Status.Equals(VacationStatus.Pending)).ToList();
            return employeeVacation;
        }
        public void UpdateEmployeeVacationStatus(Vacation vacationDays)
        {
            var vacationForUpdate = _vacationDbContext.Vacation.FirstOrDefault(vd => vd.Id == vacationDays.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacationDays.Status;

                _vacationDbContext.SaveChanges();
            }
        }
    }
}
