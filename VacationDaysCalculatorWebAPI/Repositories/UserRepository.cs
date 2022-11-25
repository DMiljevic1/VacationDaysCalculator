using DomainModel.Models;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class UserRepository
    {
        private readonly VCDDbContext _vCDDbContext;

        public UserRepository(VCDDbContext vCDDbContext)
        {
            _vCDDbContext = vCDDbContext;
        }

        public List<User> GetUsers()
        {
            return _vCDDbContext.Users.ToList();
        }

        public void InsertUser(User user)
        {
            _vCDDbContext.Users.Add(user);
            _vCDDbContext.SaveChanges();
        }

        public EmployeeDetails GetEmployeeDetails(int userId)
        {
            var employeeVacationDays = _vCDDbContext.RemainingVacationDays.FirstOrDefault(vd => vd.UserId == userId);
            var employee = _vCDDbContext.Users.FirstOrDefault(u => u.Id == userId);
            var employeeDetails = new EmployeeDetails();
            employeeDetails.FullName = employee.FirstName + " " + employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employeeVacationDays.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employeeVacationDays.RemainingDaysOffCurrentYear;
            return employeeDetails;
        }

    }
}
