using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
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

        public EmployeeDetails GetEmployeeDetails(int employeeId)
        {
            var employeeVacationDays = _vCDDbContext.RemainingVacationDays.FirstOrDefault(vd => vd.UserId == employeeId);
            var employee = _vCDDbContext.Users.FirstOrDefault(u => u.Id == employeeId);
            var employeeDetails = new EmployeeDetails();
            employeeDetails.FullName = employee.FirstName + " " + employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employeeVacationDays.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employeeVacationDays.RemainingDaysOffCurrentYear;
            return employeeDetails;
        }
        public List<EmployeeHistory> GetEmployeeHistory(int userId)
        {
            var archivedVacationDays = _vCDDbContext.VacationDays.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && vd.Status.Equals(VacationStatus.Arhived)).ToList();
            var employeeHistoryList = new List<EmployeeHistory>();
            foreach(var vacationDay in archivedVacationDays)
            {
                employeeHistoryList.Add(ConvertVacationDaysToEmployeeHistory(vacationDay));
            }
            return employeeHistoryList;
        }
        public EmployeeHistory ConvertVacationDaysToEmployeeHistory(VacationDays vacationDay)
        {
            var employeeHistory = new EmployeeHistory();
            employeeHistory.VacationFrom = vacationDay.VacationFrom;
            employeeHistory.VacationTo = vacationDay.VacationTo;
            employeeHistory.Year = vacationDay.Year;
            employeeHistory.FirstName = vacationDay.User.FirstName;
            employeeHistory.LastName = vacationDay.User.LastName;
            employeeHistory.TotalVacationSpent = CalculateTotalVacationSpentForGivenPeriod(employeeHistory.VacationFrom, employeeHistory.VacationTo);
            return employeeHistory;
        }

        private int CalculateTotalVacationSpentForGivenPeriod(DateTime vacationFrom, DateTime vacationTo)
        {
            var dayNow = DateTime.Now;
            var holidays = _vCDDbContext.Holidays.ToList();
            int totalVacationDaysSpent = 0;
            var oneDay = TimeSpan.FromDays(1);

            for (DateTime currentDay = vacationFrom; currentDay <= vacationTo; currentDay += oneDay)
            {
                if (isCurrentDayWeekend(currentDay.Date))
                    continue;
                int numberOfDaysThatAreNotOnHolidays = 0;
                foreach(var holiday in holidays)
                {
                    if (!holiday.HolidayDate.Equals(currentDay.Date))
                        numberOfDaysThatAreNotOnHolidays++;
                }
                if(numberOfDaysThatAreNotOnHolidays == holidays.Count)
                    totalVacationDaysSpent++;
            }
            return totalVacationDaysSpent;
        }

        private bool isCurrentDayWeekend(DateTime currentDay)
        {
            DayOfWeek day = currentDay.DayOfWeek;
            if(day.Equals(DayOfWeek.Sunday) || day.Equals(DayOfWeek.Saturday))
                return true;
            return false;
        }

        public void InsertVacation(VacationDays vacation)
        {
            _vCDDbContext.VacationDays.Add(vacation);
            _vCDDbContext.SaveChanges();
        }

    }
}
