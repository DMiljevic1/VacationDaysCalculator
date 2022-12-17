using DomainModel.DtoModels;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class EmployeeRepository
    {
        private readonly VCDDbContext _vCDDbContext;

        public EmployeeRepository(VCDDbContext vCDDbContext)
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
            employeeDetails.FirstName = employee.FirstName;
            employeeDetails.LastName = employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employeeVacationDays.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employeeVacationDays.RemainingDaysOffCurrentYear;
            employeeDetails.VacationDays = GetEmployeeVacation(userId);
            return employeeDetails;
        }
        public List<VacationDays> GetEmployeeVacation(int userId)
        {
            var employeeVacation = _vCDDbContext.VacationDays.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && (vd.Status.Equals(VacationStatus.Approved) || vd.Status.Equals(VacationStatus.OnVacation) || vd.Status.Equals(VacationStatus.Pending))).ToList();
            if(employeeVacation != null)
                setVacationStatus(employeeVacation);
            employeeVacation = _vCDDbContext.VacationDays.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && (vd.Status.Equals(VacationStatus.Approved) || vd.Status.Equals(VacationStatus.OnVacation) || vd.Status.Equals(VacationStatus.Pending))).ToList();
            return employeeVacation;
        }

        private void setVacationStatus(List<VacationDays> vacationDays)
        {
            var currentDate = DateTime.Today;
            var yesterday = DateTime.Today.AddDays(-1);
            foreach (var vacation in vacationDays)
            {
                if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Approved)
                {
                    vacation.Status = VacationStatus.OnVacation;
                    UpdateVacationDayStatus(vacation);
                }
                else if (vacation.VacationTo.Equals(yesterday.Date) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.OnVacation)
                {
                    vacation.Status = VacationStatus.Arhived;
                    UpdateVacationDayStatus(vacation);
                }
                else if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Pending)
                {
                    vacation.Status = VacationStatus.Cancelled;
                    UpdateVacationDayStatus(vacation);
                }
            }
        }
        public void UpdateVacationDayStatus(VacationDays vacationDays)
        {
            var vacationForUpdate = GetVacationDaysByVacationId(vacationDays.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacationDays.Status;

                _vCDDbContext.SaveChanges();
            }

        }
        public VacationDays GetVacationDaysByVacationId(int vacationId)
        {
            return _vCDDbContext.VacationDays.FirstOrDefault(vd => vd.Id == vacationId);
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
