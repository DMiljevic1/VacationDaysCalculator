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
            adminDetails.EmployeeVacationDays = GetEmployeeVacationByStatus(VacationStatus.Pending);
            return adminDetails;
        }
        public List<Vacation> GetEmployeeVacationByStatus(VacationStatus status)
        {
            var employeeVacation = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.Status.Equals(status)).ToList();
            return employeeVacation;
        }
        public void AddUser(UserDetails userDetails)
        {
            var user = new User();
            var currentYear = DateTime.Today.Year;
            user.UserName = userDetails.Username;
            user.Password = userDetails.Password;
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.CurrentYear = currentYear;
            user.RemainingDaysOffCurrentYear = userDetails.RemainingDaysOffCurrentYear;
            user.RemainingDaysOffLastYear = userDetails.RemainingDaysOffLastYear;
            user.Role = userDetails.Role;
            _vacationDbContext.Users.Add(user);
            _vacationDbContext.SaveChanges();
        }
    }
}
