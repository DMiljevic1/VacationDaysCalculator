using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class AdminRepository
    {
        private readonly VCDDbContext _vCDDbContext;

        public AdminRepository(VCDDbContext vCDDbContext)
        {
            _vCDDbContext = vCDDbContext;
        }

        public AdminDetails GetAdminDetails(int userId)
        {
            var user = _vCDDbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return null;
            var adminDetails = new AdminDetails();
            adminDetails.FirstName = user.FirstName;
            adminDetails.LastName = user.LastName;
            adminDetails.Email = user.Email;
            adminDetails.EmployeeVacationDays = GetEmployeeVacation();
            return adminDetails;
        }
        public List<VacationDays> GetEmployeeVacation()
        {
            var employeeVacation = _vCDDbContext.VacationDays.Include(vd => vd.User).Where(vd => vd.Status.Equals(VacationStatus.Pending)).ToList();
            return employeeVacation;
        }
        public void UpdateEmployeeVacationStatus(int vacationId, VacationStatus status)
        {
            var vacationForUpdate = _vCDDbContext.VacationDays.FirstOrDefault(vd => vd.Id == vacationId);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = status;

                _vCDDbContext.SaveChanges();
            }
        }
    }
}
