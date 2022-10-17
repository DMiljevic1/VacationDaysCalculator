using DomainModel.Models;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class UserLoginRepository
    {
        private readonly VCDDbContext _vCDDbContext;

        public UserLoginRepository(VCDDbContext vCDDbContext)
        {
            _vCDDbContext = vCDDbContext;
        }

        public UserLogin getCurrentUser(UserLogin userLogin)
        {

        }
    }
}
