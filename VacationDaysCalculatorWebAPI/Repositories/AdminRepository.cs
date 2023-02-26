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
        public void AddUser(User user)
        {
            _vacationDbContext.Users.Add(user);
            _vacationDbContext.SaveChanges();
        }
        public List<Vacation> GetApprovedVacations()
        {
            return _vacationDbContext.Vacation.Include(v => v.User).Where(v => v.Status == VacationStatus.Approved || v.Status == VacationStatus.OnVacation || v.Status == VacationStatus.Arhived).ToList();
        }
        public List<Vacation> GetEmployeeVacationByStatus(VacationStatus status)
        {
            return _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.Status.Equals(status)).ToList();
        }
        public User GetUserById(int userId)
        {
            return _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        }
        public List<User> GetUsers()
        {
            return _vacationDbContext.Users.ToList();
        }
    }
}
