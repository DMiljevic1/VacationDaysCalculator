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

        public void insertUser(User user)
        {
            _vCDDbContext.Users.Add(user);
            _vCDDbContext.SaveChanges();
        }

        public User getUser(int userId)
        {
            return _vCDDbContext.Users.FirstOrDefault(user => user.Id == userId);
        }

    }
}
