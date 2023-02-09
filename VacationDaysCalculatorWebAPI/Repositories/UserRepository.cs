using DomainModel.Models;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class UserRepository
    {
        private readonly VacationDbContext _vacationDbContext;
        public UserRepository(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }
        public User GetUser(int userId)
        {
            return _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        }
        public void SaveChanges()
        {
            _vacationDbContext.SaveChanges();  
        }
    }
}
